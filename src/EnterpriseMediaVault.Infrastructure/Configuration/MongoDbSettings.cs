namespace EnterpriseMediaVault.Infrastructure.Configuration;

public sealed class MongoDbSettings
{
    public const string SectionName = "MongoDb";
    public string ConnectionString { get; init; } = "mongodb://localhost:27017";
    public string DatabaseName { get; init; } = "enterprise_media_vault";
    public string GridFsBucketName { get; init; } = "vault_files";
}
