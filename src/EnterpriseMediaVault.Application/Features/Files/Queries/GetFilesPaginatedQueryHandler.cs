using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed class GetFilesPaginatedQueryHandler(
    IMongoRepository<FileDocument> files,
    ICurrentUserService currentUser)
    : IRequestHandler<GetFilesPaginatedQuery, ApiResponse<PagedResult<FileDto>>>
{
    public async Task<ApiResponse<PagedResult<FileDto>>> Handle(GetFilesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = await files.FilterAsync(q =>
        {
            var predicate = q.Where(f => !f.SoftDelete && f.TenantId == currentUser.TenantId);

            if (!string.IsNullOrWhiteSpace(request.FolderId))
            {
                predicate = predicate.Where(f => f.FolderId == request.FolderId);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                predicate = predicate.Where(f =>
                    f.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            return predicate;
        }, cancellationToken);

        var totalCount = query.Count();
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsEnumerable()
            .Adapt<List<FileDto>>();

        var pagedResult = new PagedResult<FileDto>
        {
            Items = items,
            Total = totalCount,
            Page = request.PageNumber,
            PageSize = request.PageSize
        };

        return ApiResponse<PagedResult<FileDto>>.Ok(pagedResult, "Archivos obtenidos");
    }
}
