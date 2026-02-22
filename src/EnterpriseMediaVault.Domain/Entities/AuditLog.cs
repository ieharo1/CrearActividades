using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class AuditLog : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string TenantId { get; set; } = "default";
}
