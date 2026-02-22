using System.Text.Json;
using Carter;
using EnterpriseMediaVault.API.Hubs;
using EnterpriseMediaVault.API.Extensions;
using EnterpriseMediaVault.Application.Features.Files.Commands;
using EnterpriseMediaVault.Application.Features.Files.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace EnterpriseMediaVault.API.Modules;

public sealed class FilesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/files").RequireAuthorization().WithTags("Files");

        group.MapPost("/upload", UploadAsync)
            .DisableAntiforgery()
            .RequireAuthorization("ManagersOrAdmin")
            .Accepts<IFormFile>("multipart/form-data");

        group.MapGet("/{fileId}/download", async (string fileId, string? strategy, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DownloadFileQuery(fileId, strategy ?? "gridfs"), ct);
            if (!result.Success || result.Data is null)
            {
                return Results.NotFound(result);
            }

            return Results.File(result.Data.Stream, result.Data.MimeType, result.Data.Name, enableRangeProcessing: true);
        }).RequireAuthorization("perm:read:File");

        group.MapPost("/{fileId}/rollback/{version:int}", async (string fileId, int version, string? strategy, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new RollbackFileVersionCommand(fileId, version, strategy ?? "gridfs"), ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization("ManagersOrAdmin");
    }

    private static async Task<IResult> UploadAsync(HttpRequest request, ISender sender, IHubContext<NotificationHub> hub, CancellationToken ct)
    {
        if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader))
        {
            return Results.BadRequest("Invalid content type");
        }

        var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary).Value;
        if (string.IsNullOrWhiteSpace(boundary))
        {
            return Results.BadRequest("Missing multipart boundary");
        }

        var reader = new MultipartReader(boundary, request.Body);
        var metadata = new Dictionary<string, object>();
        string? fileName = null;
        string mimeType = "application/octet-stream";
        string folderId = string.Empty;
        Stream? fileStream = null;
        string? tempPath = null;
        long fileSize = 0;

        MultipartSection? section;
        while ((section = await reader.ReadNextSectionAsync(ct)) is not null)
        {
            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
            {
                continue;
            }

            var fieldName = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
            if (contentDisposition.FileName.HasValue || contentDisposition.FileNameStar.HasValue)
            {
                fileName = HeaderUtilities.RemoveQuotes(contentDisposition.FileNameStar.HasValue ? contentDisposition.FileNameStar : contentDisposition.FileName).Value;
                mimeType = section.ContentType ?? "application/octet-stream";

                if (!MimeSecurity.IsAllowedMime(mimeType))
                {
                    return Results.BadRequest($"MIME type no permitido: {mimeType}");
                }

                tempPath = Path.GetTempFileName();
                await using var fs = File.Create(tempPath);
                await section.Body.CopyToAsync(fs, ct);
                fileSize = fs.Length;
                fileStream = File.OpenRead(tempPath);
            }
            else
            {
                using var readerField = new StreamReader(section.Body);
                var value = await readerField.ReadToEndAsync();
                if (string.Equals(fieldName, "folderId", StringComparison.OrdinalIgnoreCase))
                {
                    folderId = value;
                }
                else if (string.Equals(fieldName, "metadata", StringComparison.OrdinalIgnoreCase))
                {
                    var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(value);
                    if (parsed is not null)
                    {
                        metadata = parsed;
                    }
                }
                else
                {
                    metadata[fieldName ?? "unknown"] = value;
                }
            }
        }

        if (fileStream is null || string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderId))
        {
            return Results.BadRequest("Archivo y folderId son requeridos");
        }

        await using (fileStream)
        {
            var command = new UploadFileCommand(fileName, mimeType, fileSize, folderId, fileStream, metadata, "gridfs");
            var result = await sender.Send(command, ct);
            if (result.Success)
            {
                await hub.Clients.All.SendAsync("fileUploaded", new { result.Data?.Id, result.Data?.Name, result.Data?.CurrentVersion }, ct);
                if (!string.IsNullOrWhiteSpace(tempPath) && File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                return Results.Ok(result);
            }
            if (!string.IsNullOrWhiteSpace(tempPath) && File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            return Results.BadRequest(result);
        }
    }
}
