using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Search.Queries;

public sealed record SearchFilesQuery(
    string? Text,
    string? FolderId,
    string? Type,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "updatedAtUtc",
    bool Desc = true)
    : IRequest<ApiResponse<PagedResult<FileDto>>>;
