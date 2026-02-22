namespace EnterpriseMediaVault.Domain.Common;

public abstract class AuditableEntity : AggregateRoot
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool SoftDelete { get; set; }
}
