using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed class GetFileByIdQueryHandler(IMongoRepository<FileDocument> files)
    : IRequestHandler<GetFileByIdQuery, ApiResponse<FileDto>>
{
    public async Task<ApiResponse<FileDto>> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await files.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null)
        {
            return ApiResponse<FileDto>.Fail("Archivo no encontrado", "NOT_FOUND");
        }

        return ApiResponse<FileDto>.Ok(file.Adapt<FileDto>(), "Archivo obtenido");
    }
}
