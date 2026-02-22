using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Search.Queries;

public sealed class SearchFilesQueryHandler(IMongoRepository<FileDocument> files, ICurrentUserService currentUser)
    : IRequestHandler<SearchFilesQuery, ApiResponse<PagedResult<FileDto>>>
{
    public async Task<ApiResponse<PagedResult<FileDto>>> Handle(SearchFilesQuery request, CancellationToken cancellationToken)
    {
        var data = await files.FilterAsync(q => q.Where(f => !f.SoftDelete && f.TenantId == currentUser.TenantId), cancellationToken);
        var query = data.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            var text = request.Text.ToLowerInvariant();
            query = query.Where(f => f.Name.ToLower().Contains(text) || f.Type.ToLower().Contains(text) || f.MimeType.ToLower().Contains(text));
        }

        if (!string.IsNullOrWhiteSpace(request.FolderId))
        {
            query = query.Where(f => f.FolderId == request.FolderId);
        }

        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            var typeFilter = request.Type.ToLowerInvariant();
            query = query.Where(f => f.MimeType.ToLower().Contains(typeFilter) || f.Type.ToLower().Contains(typeFilter));
        }

        query = request.SortBy.ToLowerInvariant() switch
        {
            "name" => request.Desc ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name),
            "size" => request.Desc ? query.OrderByDescending(f => f.Size) : query.OrderBy(f => f.Size),
            _ => request.Desc ? query.OrderByDescending(f => f.UpdatedAtUtc) : query.OrderBy(f => f.UpdatedAtUtc)
        };

        var total = query.LongCount();
        var items = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

        return ApiResponse<PagedResult<FileDto>>.Ok(new PagedResult<FileDto>
        {
            Items = items.Select(f => new FileDto
            {
                Id = f.Id,
                Name = f.Name,
                MimeType = f.MimeType,
                Type = f.Type,
                Size = f.Size,
                CurrentVersion = f.CurrentVersion,
                Hash = f.Hash,
                FolderId = f.FolderId,
                OwnerId = f.OwnerId,
                CreatedAt = f.CreatedAtUtc,
                ModifiedAt = f.UpdatedAtUtc
            }).ToList(),
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
