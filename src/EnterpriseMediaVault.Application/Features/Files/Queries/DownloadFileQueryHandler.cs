using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed class DownloadFileQueryHandler(
    IMongoRepository<FileDocument> files,
    IStorageStrategyResolver storageResolver,
    IAuditService auditService)
    : IRequestHandler<DownloadFileQuery, ApiResponse<DownloadFileResult>>
{
    public async Task<ApiResponse<DownloadFileResult>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var file = await files.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null || file.SoftDelete)
        {
            return ApiResponse<DownloadFileResult>.Fail("Archivo no encontrado", "NOT_FOUND");
        }

        var stream = await storageResolver.Resolve(request.StorageStrategy).OpenReadAsync(file.CurrentStorageReference, cancellationToken);
        file.DownloadCount += 1;
        file.UpdatedAtUtc = DateTime.UtcNow;
        await files.ReplaceAsync(file, cancellationToken);
        await auditService.WriteAsync("DOWNLOAD_FILE", "File", file.Id, file.Name, cancellationToken);

        return ApiResponse<DownloadFileResult>.Ok(new DownloadFileResult
        {
            Name = file.Name,
            MimeType = file.MimeType,
            Stream = stream
        });
    }
}
