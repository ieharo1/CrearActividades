using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed class UpdateFileCommandHandler(
    IMongoRepository<FileDocument> files,
    IAuditService auditService)
    : IRequestHandler<UpdateFileCommand, ApiResponse<FileDto>>
{
    public async Task<ApiResponse<FileDto>> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var file = await files.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null)
        {
            return ApiResponse<FileDto>.Fail("Archivo no encontrado", "NOT_FOUND");
        }

        file.Name = request.Name;
        file.Metadata = request.Metadata;

        await files.ReplaceAsync(file, cancellationToken);
        await auditService.WriteAsync("UPDATE_FILE", "File", file.Id, file.Name, cancellationToken);

        return ApiResponse<FileDto>.Ok(file.Adapt<FileDto>(), "Archivo actualizado");
    }
}
