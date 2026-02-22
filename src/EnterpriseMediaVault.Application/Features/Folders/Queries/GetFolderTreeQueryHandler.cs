using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Queries;

public sealed class GetFolderTreeQueryHandler(IMongoRepository<Folder> folders, ICurrentUserService currentUser)
    : IRequestHandler<GetFolderTreeQuery, ApiResponse<IReadOnlyCollection<FolderDto>>>
{
    public async Task<ApiResponse<IReadOnlyCollection<FolderDto>>> Handle(GetFolderTreeQuery request, CancellationToken cancellationToken)
    {
        var allFolders = await folders.FilterAsync(
            q => q.Where(f => f.TenantId == currentUser.TenantId && !f.SoftDelete),
            cancellationToken);

        var folderDtos = allFolders.Select(f => new FolderDto
        {
            Id = f.Id,
            Name = f.Name,
            ParentFolderId = f.ParentFolderId,
            OwnerId = f.OwnerId,
            ParentId = f.ParentFolderId
        }).ToList();

        return ApiResponse<IReadOnlyCollection<FolderDto>>.Ok(folderDtos);
    }
}
