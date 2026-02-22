namespace EnterpriseMediaVault.Application.DTOs;

public sealed class UpdateFolderDto
{
    public string Name { get; init; } = string.Empty;
    public string? ParentFolderId { get; init; }
}
