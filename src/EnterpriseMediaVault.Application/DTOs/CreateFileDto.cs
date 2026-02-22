namespace EnterpriseMediaVault.Application.DTOs;

public sealed class CreateFileDto
{
    public string Name { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
    public long Size { get; init; }
    public string FolderId { get; init; } = string.Empty;
    public Dictionary<string, object> Metadata { get; init; } = new();
}
