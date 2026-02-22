namespace EnterpriseMediaVault.Application.DTOs;

public sealed class CreateFolderDto
{
    public string Name { get; init; } = string.Empty;
    public string? ParentFolderId { get; init; }
}
