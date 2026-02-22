using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Domain.Common;
using EnterpriseMediaVault.Infrastructure.Mongo;
using MongoDB.Driver;

namespace EnterpriseMediaVault.Infrastructure.Repositories;

public sealed class MongoRepository<T>(IMongoDbContext context) : IMongoRepository<T> where T : Entity
{
    private readonly IMongoCollection<T> _collection = context.Database.GetCollection<T>(ResolveCollectionName());

    public IQueryable<T> AsQueryable() => _collection.AsQueryable();

    public async Task<long> CountAsync(Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null, CancellationToken cancellationToken = default)
    {
        var query = queryBuilder is null ? AsQueryable() : queryBuilder(AsQueryable());
        return await Task.FromResult(query.LongCount());
    }

    public async Task DeleteSoftAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        if (entity is AuditableEntity auditable)
        {
            auditable.SoftDelete = true;
            auditable.UpdatedAtUtc = DateTime.UtcNow;
            await ReplaceAsync(entity, cancellationToken);
            return;
        }

        await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<T>> FilterAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default)
    {
        var query = queryBuilder(AsQueryable());
        return await Task.FromResult<IReadOnlyCollection<T>>(query.ToList());
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task ReplaceAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
    }

    private static string ResolveCollectionName()
    {
        var name = typeof(T).Name;
        return name switch
        {
            "User" => "users",
            "Role" => "roles",
            "Folder" => "folders",
            "FileDocument" => "files",
            "FileVersion" => "fileVersions",
            "AuditLog" => "auditLogs",
            "Permission" => "permissions",
            "Tag" => "tags",
            "RefreshToken" => "refreshTokens",
            _ => char.ToLowerInvariant(name[0]) + name[1..] + "s"
        };
    }
}
