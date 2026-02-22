using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Commands;

public sealed class DeleteFolderCommandHandler(IMongoRepository<Folder> folders, IAuditService auditService)
    : IRequestHandler<DeleteFolderCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = await folders.GetByIdAsync(request.FolderId, cancellationToken);
        if (folder is null)
        {
            return ApiResponse<bool>.Fail("Carpeta no encontrada", "NOT_FOUND");
        }

        await folders.DeleteSoftAsync(request.FolderId, cancellationToken);
        await auditService.WriteAsync("DELETE_FOLDER", "Folder", request.FolderId, "Soft delete", cancellationToken);
        return ApiResponse<bool>.Ok(true, "Carpeta eliminada");
    }
}
