using Carter;
using EnterpriseMediaVault.Application.Features.Folders.Commands;
using EnterpriseMediaVault.Application.Features.Folders.Queries;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.API.Modules;

public sealed class FoldersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/folders").RequireAuthorization().WithTags("Folders");

        group.MapPost("/", async (CreateFolderCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization("ManagersOrAdmin");

        group.MapDelete("/{folderId}", async (string folderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteFolderCommand(folderId), ct);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }).RequireAuthorization("ManagersOrAdmin");

        group.MapGet("/tree", async (string? rootFolderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetFolderTreeQuery(rootFolderId), ct);
            return Results.Ok(result);
        });

        group.MapGet("/contents", async (string? folderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetFolderContentsQuery(folderId), ct);
            return Results.Ok(result);
        });

        group.MapGet("/root", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetFolderContentsQuery(null), ct);
            return Results.Ok(result);
        });
        
        group.MapGet("/{folderId}", async (string folderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetFolderContentsQuery(folderId), ct);
            return Results.Ok(result);
        }).AllowAnonymous();

        group.MapPut("/{folderId}", async (string folderId, UpdateFolderDto dto, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new UpdateFolderCommand(folderId, dto.Name, dto.ParentFolderId), ct);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization("ManagersOrAdmin");
    }
}
