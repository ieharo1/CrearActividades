using EnterpriseMediaVault.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace EnterpriseMediaVault.Infrastructure.Mongo;

public sealed class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }
    public GridFSBucket GridFsBucket { get; }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        Database = client.GetDatabase(settings.Value.DatabaseName);
        GridFsBucket = new GridFSBucket(Database, new GridFSBucketOptions { BucketName = settings.Value.GridFsBucketName });
    }
}
