namespace EnterpriseMediaVault.Application.Abstractions;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}
