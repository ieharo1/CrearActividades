using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;
using EnterpriseMediaVault.Domain.Entities;
using EnterpriseMediaVault.Application.Abstractions;

namespace EnterpriseMediaVault.Application.Features.Folders.Queries;

public sealed record GetFolderContentsQuery(string? FolderId) : IRequest<ApiResponse<FolderContentsDto>>;

public sealed class FolderContentsDto
{
    public FolderDto? Folder { get; set; }
    public List<FolderDto> SubFolders { get; set; } = new();
    public List<FileDto> Files { get; set; } = new();
}

public sealed class GetFolderContentsQueryHandler(
    IMongoRepository<Folder> folders,
    IMongoRepository<FileDocument> files,
    ICurrentUserService currentUser)
    : IRequestHandler<GetFolderContentsQuery, ApiResponse<FolderContentsDto>>
{
    public async Task<ApiResponse<FolderContentsDto>> Handle(GetFolderContentsQuery request, CancellationToken ct)
    {
        var tenantId = currentUser.TenantId;
        FolderDto? folderDto = null;

        if (!string.IsNullOrEmpty(request.FolderId))
        {
            var folder = await folders.GetByIdAsync(request.FolderId, ct);
            if (folder == null || folder.TenantId != tenantId)
            {
                return ApiResponse<FolderContentsDto>.Fail("Carpeta no encontrada");
            }
            folderDto = new FolderDto { Id = folder.Id, Name = folder.Name, ParentFolderId = folder.ParentFolderId, CreatedAt = folder.CreatedAtUtc };
        }

        var subFolders = await folders.FilterAsync(f => f.Where(x => 
            x.TenantId == tenantId && 
            !x.SoftDelete && 
            (request.FolderId == null ? x.ParentFolderId == null : x.ParentFolderId == request.FolderId)), ct);

        var filesInFolder = await files.FilterAsync(f => f.Where(x => 
            x.TenantId == tenantId && 
            !x.SoftDelete && 
            (request.FolderId == null ? string.IsNullOrEmpty(x.FolderId) : x.FolderId == request.FolderId)), ct);

        var result = new FolderContentsDto
        {
            Folder = folderDto,
            SubFolders = subFolders.Select(f => new FolderDto 
            { 
                Id = f.Id, 
                Name = f.Name, 
                ParentFolderId = f.ParentFolderId,
                ItemCount = filesInFolder.Count(x => x.FolderId == f.Id),
                CreatedAt = f.CreatedAtUtc
            }).ToList(),
            Files = filesInFolder.Select(f => new FileDto
            {
                Id = f.Id,
                Name = f.Name,
                MimeType = f.MimeType,
                Size = f.Size,
                CurrentVersion = f.CurrentVersion,
                CreatedAt = f.CreatedAtUtc,
                ModifiedAt = f.UpdatedAtUtc
            }).ToList()
        };

        return ApiResponse<FolderContentsDto>.Ok(result);
    }
}
