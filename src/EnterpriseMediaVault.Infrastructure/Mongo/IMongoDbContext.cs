using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace EnterpriseMediaVault.Infrastructure.Mongo;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    GridFSBucket GridFsBucket { get; }
}
