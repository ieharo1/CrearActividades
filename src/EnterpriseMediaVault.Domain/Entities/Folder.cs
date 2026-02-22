using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class Folder : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? ParentFolderId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string TenantId { get; set; } = "default";
    public bool InheritPermissions { get; set; } = true;
}
