using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Queries;

public sealed record GetFolderByIdQuery(string FolderId) : IRequest<ApiResponse<FolderDto>>;
