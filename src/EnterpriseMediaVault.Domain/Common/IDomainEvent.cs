namespace EnterpriseMediaVault.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
