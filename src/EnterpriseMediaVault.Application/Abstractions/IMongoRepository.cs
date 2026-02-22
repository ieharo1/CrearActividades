using EnterpriseMediaVault.Domain.Common;

namespace EnterpriseMediaVault.Application.Abstractions;

public interface IMongoRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<T>> FilterAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default);
    Task<long> CountAsync(Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null, CancellationToken cancellationToken = default);
    Task InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task ReplaceAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteSoftAsync(string id, CancellationToken cancellationToken = default);
    IQueryable<T> AsQueryable();
}
