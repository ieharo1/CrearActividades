namespace EnterpriseMediaVault.Domain.Common;

public abstract class Entity
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
}
