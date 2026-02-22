using EnterpriseMediaVault.Domain.Entities;
using EnterpriseMediaVault.Infrastructure.Mongo;
using MongoDB.Driver;

namespace EnterpriseMediaVault.Infrastructure.Services;

public sealed class MongoIndexInitializer(IMongoDbContext context)
{
    public async Task EnsureIndexesAsync(CancellationToken cancellationToken = default)
    {
        var files = context.Database.GetCollection<FileDocument>("files");
        var fileIndexes = new List<CreateIndexModel<FileDocument>>
        {
            new(Builders<FileDocument>.IndexKeys.Ascending(x => x.TenantId).Ascending(x => x.FolderId).Ascending(x => x.Name)),
            new(Builders<FileDocument>.IndexKeys.Text(x => x.Name).Text(x => x.MimeType).Text(x => x.Type)),
            new(Builders<FileDocument>.IndexKeys.Ascending(x => x.UpdatedAtUtc)),
            new(Builders<FileDocument>.IndexKeys.Ascending(x => x.SoftDelete))
        };
        await files.Indexes.CreateManyAsync(fileIndexes, cancellationToken);

        var folders = context.Database.GetCollection<Folder>("folders");
        var folderIndexes = new List<CreateIndexModel<Folder>>
        {
            new(Builders<Folder>.IndexKeys.Ascending(x => x.TenantId).Ascending(x => x.ParentFolderId).Ascending(x => x.Name)),
            new(Builders<Folder>.IndexKeys.Ascending(x => x.SoftDelete))
        };
        await folders.Indexes.CreateManyAsync(folderIndexes, cancellationToken);

        var auditLogs = context.Database.GetCollection<AuditLog>("auditLogs");
        await auditLogs.Indexes.CreateOneAsync(new CreateIndexModel<AuditLog>(Builders<AuditLog>.IndexKeys.Descending(x => x.CreatedAtUtc)), cancellationToken: cancellationToken);

        var refreshTokens = context.Database.GetCollection<RefreshToken>("refreshTokens");
        var ttlOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(1) };
        await refreshTokens.Indexes.CreateOneAsync(
            new CreateIndexModel<RefreshToken>(Builders<RefreshToken>.IndexKeys.Ascending(x => x.ExpiresAtUtc), ttlOptions), cancellationToken: cancellationToken);
    }
}
