using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Commands;

public sealed class UpdateFolderCommandHandler(
    IMongoRepository<Folder> folders,
    IAuditService auditService)
    : IRequestHandler<UpdateFolderCommand, ApiResponse<FolderDto>>
{
    public async Task<ApiResponse<FolderDto>> Handle(UpdateFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = await folders.GetByIdAsync(request.FolderId, cancellationToken);
        if (folder is null)
        {
            return ApiResponse<FolderDto>.Fail("Carpeta no encontrada", "NOT_FOUND");
        }

        folder.Name = request.Name;
        folder.ParentFolderId = request.ParentFolderId;

        await folders.ReplaceAsync(folder, cancellationToken);
        await auditService.WriteAsync("UPDATE_FOLDER", "Folder", folder.Id, folder.Name, cancellationToken);

        return ApiResponse<FolderDto>.Ok(folder.Adapt<FolderDto>(), "Carpeta actualizada");
    }
}
