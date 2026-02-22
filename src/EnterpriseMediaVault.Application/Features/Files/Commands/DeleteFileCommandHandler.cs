using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed class DeleteFileCommandHandler(
    IMongoRepository<FileDocument> files,
    IAuditService auditService)
    : IRequestHandler<DeleteFileCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var file = await files.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null)
        {
            return ApiResponse<bool>.Fail("Archivo no encontrado", "NOT_FOUND");
        }

        await files.DeleteSoftAsync(request.FileId, cancellationToken);
        await auditService.WriteAsync("DELETE_FILE", "File", request.FileId, "Soft delete", cancellationToken);

        return ApiResponse<bool>.Ok(true, "Archivo eliminado");
    }
}
