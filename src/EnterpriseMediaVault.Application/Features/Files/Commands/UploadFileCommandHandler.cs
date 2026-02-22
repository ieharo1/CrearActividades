using System.Security.Cryptography;
using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed class UploadFileCommandHandler(
    IMongoRepository<FileDocument> files,
    IMongoRepository<FileVersion> versions,
    IStorageStrategyResolver storageResolver,
    ICurrentUserService currentUser,
    IAuditService auditService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UploadFileCommand, ApiResponse<FileDto>>
{
    public async Task<ApiResponse<FileDto>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var storage = storageResolver.Resolve(request.StorageStrategy);

        var existing = (await files.FilterAsync(
            q => q.Where(f => !f.SoftDelete && f.FolderId == request.FolderId && f.Name == request.FileName && f.TenantId == currentUser.TenantId),
            cancellationToken)).FirstOrDefault();

        string hash;
        using (var sha = SHA256.Create())
        {
            hash = Convert.ToHexString(await sha.ComputeHashAsync(request.FileStream, cancellationToken));
        }

        if (request.FileStream.CanSeek)
        {
            request.FileStream.Position = 0;
        }
        else
        {
            return ApiResponse<FileDto>.Fail("El stream de archivo no permite reposicionamiento", "NON_SEEKABLE_STREAM");
        }

        var storageReference = await storage.SaveAsync(request.FileName, request.MimeType, request.FileStream, request.Metadata, cancellationToken);

        if (existing is null)
        {
            var file = new FileDocument
            {
                Name = request.FileName,
                Type = Path.GetExtension(request.FileName).TrimStart('.').ToLowerInvariant(),
                MimeType = request.MimeType,
                Size = request.Size,
                Hash = hash,
                Metadata = request.Metadata,
                OwnerId = currentUser.UserId,
                FolderId = request.FolderId,
                CurrentStorageReference = storageReference,
                TenantId = currentUser.TenantId
            };

            var v1 = new FileVersion
            {
                FileId = file.Id,
                VersionNumber = 1,
                StorageReference = storageReference,
                Hash = hash,
                Size = request.Size,
                MimeType = request.MimeType,
                UploadedBy = currentUser.UserId
            };

            await unitOfWork.ExecuteAsync(async () =>
            {
                await files.InsertAsync(file, cancellationToken);
                await versions.InsertAsync(v1, cancellationToken);
            }, cancellationToken);

            await auditService.WriteAsync("UPLOAD_FILE", "File", file.Id, request.FileName, cancellationToken);
            return ApiResponse<FileDto>.Ok(file.Adapt<FileDto>(), "Archivo cargado");
        }

        existing.RegisterNewVersion(storageReference, hash, request.Size, request.MimeType);
        await files.ReplaceAsync(existing, cancellationToken);

        await versions.InsertAsync(new FileVersion
        {
            FileId = existing.Id,
            VersionNumber = existing.CurrentVersion,
            StorageReference = storageReference,
            Hash = hash,
            Size = request.Size,
            MimeType = request.MimeType,
            UploadedBy = currentUser.UserId
        }, cancellationToken);

        await auditService.WriteAsync("NEW_FILE_VERSION", "File", existing.Id, $"Version {existing.CurrentVersion}", cancellationToken);
        return ApiResponse<FileDto>.Ok(existing.Adapt<FileDto>(), "Nueva versión cargada");
    }
}
