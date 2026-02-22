using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed record GetFilesPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? FolderId = null,
    string? SearchTerm = null) : IRequest<ApiResponse<PagedResult<FileDto>>>;
