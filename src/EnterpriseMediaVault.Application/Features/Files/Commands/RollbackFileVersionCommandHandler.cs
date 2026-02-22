using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed class RollbackFileVersionCommandHandler(
    IMongoRepository<FileDocument> files,
    IMongoRepository<FileVersion> versions,
    IAuditService auditService)
    : IRequestHandler<RollbackFileVersionCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(RollbackFileVersionCommand request, CancellationToken cancellationToken)
    {
        var file = await files.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null)
        {
            return ApiResponse<bool>.Fail("Archivo no encontrado", "NOT_FOUND");
        }

        var target = (await versions.FilterAsync(
            q => q.Where(v => v.FileId == request.FileId && v.VersionNumber == request.VersionNumber && !v.SoftDelete),
            cancellationToken)).FirstOrDefault();

        if (target is null)
        {
            return ApiResponse<bool>.Fail("Versión no encontrada", "VERSION_NOT_FOUND");
        }

        file.CurrentVersion = target.VersionNumber;
        file.CurrentStorageReference = target.StorageReference;
        file.Hash = target.Hash;
        file.Size = target.Size;
        file.MimeType = target.MimeType;
        file.UpdatedAtUtc = DateTime.UtcNow;

        await files.ReplaceAsync(file, cancellationToken);
        await auditService.WriteAsync("ROLLBACK_FILE", "File", request.FileId, $"Rollback to version {request.VersionNumber}", cancellationToken);

        return ApiResponse<bool>.Ok(true, "Rollback exitoso");
    }
}
