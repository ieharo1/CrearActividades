using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using EnterpriseMediaVault.Domain.Entities;
using Mapster;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Folders.Queries;

public sealed class GetFolderByIdQueryHandler(IMongoRepository<Folder> folders)
    : IRequestHandler<GetFolderByIdQuery, ApiResponse<FolderDto>>
{
    public async Task<ApiResponse<FolderDto>> Handle(GetFolderByIdQuery request, CancellationToken cancellationToken)
    {
        var folder = await folders.GetByIdAsync(request.FolderId, cancellationToken);
        if (folder is null)
        {
            return ApiResponse<FolderDto>.Fail("Carpeta no encontrada", "NOT_FOUND");
        }

        return ApiResponse<FolderDto>.Ok(folder.Adapt<FolderDto>(), "Carpeta obtenida");
    }
}
