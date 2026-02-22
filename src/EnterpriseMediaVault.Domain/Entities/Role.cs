using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Entities;

public sealed class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
