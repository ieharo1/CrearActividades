using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class FileVersion : AuditableEntity
{
    public string FileId { get; set; } = string.Empty;
    public int VersionNumber { get; set; }
    public string StorageReference { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public long Size { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string UploadedBy { get; set; } = string.Empty;
}
