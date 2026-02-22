using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class Tag : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#1f2937";
    public string TenantId { get; set; } = "default";
}
