using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Commands;

public sealed record CreateFolderCommand(string Name, string? ParentFolderId) : IRequest<ApiResponse<FolderDto>>;
