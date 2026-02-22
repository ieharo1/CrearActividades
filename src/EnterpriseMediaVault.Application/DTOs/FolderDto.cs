namespace EnterpriseMediaVault.Application.DTOs;

public sealed class FolderDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? ParentId { get; init; }
    public string? ParentFolderId { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public int ItemCount { get; init; }
    public DateTime? CreatedAt { get; init; }
    public IReadOnlyCollection<FolderDto> Children { get; init; } = Array.Empty<FolderDto>();
}
