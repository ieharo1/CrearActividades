using EnterpriseMediaVault.Domain.Common;
using EnterpriseMediaVault.Domain.Events;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class FileDocument : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Hash { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
    public string OwnerId { get; set; } = string.Empty;
    public string FolderId { get; set; } = string.Empty;
    public int CurrentVersion { get; set; } = 1;
    public string CurrentStorageReference { get; set; } = string.Empty;
    public string TenantId { get; set; } = "default";
    public long DownloadCount { get; set; }

    public void RegisterNewVersion(string storageReference, string hash, long size, string mimeType)
    {
        CurrentVersion++;
        CurrentStorageReference = storageReference;
        Hash = hash;
        Size = size;
        MimeType = mimeType;
        UpdatedAtUtc = DateTime.UtcNow;
        AddDomainEvent(new FileVersionCreatedDomainEvent(Id, CurrentVersion));
    }
}
