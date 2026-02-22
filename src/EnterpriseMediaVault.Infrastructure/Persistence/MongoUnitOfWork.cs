using EnterpriseMediaVault.Application.Abstractions;

namespace EnterpriseMediaVault.Infrastructure.Persistence;

public sealed class MongoUnitOfWork : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await action();
    }
}
