using Carter;
using EnterpriseMediaVault.Application.Features.Search.Queries;
using MediatR;

namespace EnterpriseMediaVault.API.Modules;

public sealed class SearchModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/search").RequireAuthorization().WithTags("Search");

        group.MapGet("", async (
            string? q,
            string? type,
            string? folderId,
            int page,
            int pageSize,
            string? sortBy,
            bool desc,
            ISender sender,
            CancellationToken ct) =>
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 50 : pageSize;
            var result = await sender.Send(new SearchFilesQuery(q, folderId, type, page, pageSize, sortBy ?? "createdAt", desc), ct);
            return Results.Ok(result);
        });

        group.MapGet("/files", async (
            string? text,
            string? folderId,
            int page,
            int pageSize,
            string sortBy,
            bool desc,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new SearchFilesQuery(text, folderId, null, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize, sortBy, desc), ct);
            return Results.Ok(result);
        });
    }
}
