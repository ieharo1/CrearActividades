namespace EnterpriseMediaVault.Application.DTOs;

public sealed class FileDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
    public long Size { get; init; }
    public string Hash { get; init; } = string.Empty;
    public string FolderId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public int CurrentVersion { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
