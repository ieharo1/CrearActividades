using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Queries;

public sealed record GetFolderTreeQuery(string? RootFolderId) : IRequest<ApiResponse<IReadOnlyCollection<FolderDto>>>;
