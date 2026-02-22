using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed class CreateFileCommandHandler(
    IMongoRepository<FileDocument> files,
    ICurrentUserService currentUser,
    IAuditService auditService)
    : IRequestHandler<CreateFileCommand, ApiResponse<FileDto>>
{
    public async Task<ApiResponse<FileDto>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        var file = new FileDocument
        {
            Name = request.Name,
            Type = Path.GetExtension(request.Name).TrimStart('.').ToLowerInvariant(),
            MimeType = request.MimeType,
            Size = request.Size,
            FolderId = request.FolderId,
            Metadata = request.Metadata,
            OwnerId = currentUser.UserId,
            TenantId = currentUser.TenantId
        };

        await files.InsertAsync(file, cancellationToken);
        await auditService.WriteAsync("CREATE_FILE", "File", file.Id, file.Name, cancellationToken);

        return ApiResponse<FileDto>.Ok(file.Adapt<FileDto>(), "Archivo creado");
    }
}
