using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class Permission : AuditableEntity
{
    public string ResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public string SubjectId { get; set; } = string.Empty;
    public string SubjectType { get; set; } = "User";
    public string Action { get; set; } = string.Empty;
    public bool Allowed { get; set; } = true;
    public string TenantId { get; set; } = "default";
}
