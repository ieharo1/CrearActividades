using EnterpriseMediaVault.Application.Common;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Commands;

public sealed record DeleteFolderCommand(string FolderId) : IRequest<ApiResponse<bool>>;
