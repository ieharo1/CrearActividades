using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Commands;

public sealed class CreateFolderCommandHandler(
    IMongoRepository<Folder> folders,
    ICurrentUserService currentUser,
    IAuditService auditService)
    : IRequestHandler<CreateFolderCommand, ApiResponse<FolderDto>>
{
    public async Task<ApiResponse<FolderDto>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = new Folder
        {
            Name = request.Name,
            ParentFolderId = request.ParentFolderId,
            OwnerId = currentUser.UserId,
            TenantId = currentUser.TenantId
        };

        await folders.InsertAsync(folder, cancellationToken);
        await auditService.WriteAsync("CREATE_FOLDER", "Folder", folder.Id, folder.Name, cancellationToken);

        return ApiResponse<FolderDto>.Ok(folder.Adapt<FolderDto>(), "Carpeta creada");
    }
}
