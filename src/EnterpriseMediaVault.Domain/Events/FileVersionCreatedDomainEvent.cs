using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Domain.Events;

public sealed record FileVersionCreatedDomainEvent(string FileId, int Version) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
