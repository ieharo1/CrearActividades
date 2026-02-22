namespace EnterpriseMediaVault.Infrastructure.Configuration;

public sealed class StorageSettings
{
    public const string SectionName = "Storage";
    public string DefaultStrategy { get; init; } = "gridfs";
    public string LocalPath { get; init; } = "storage";
    public string S3Bucket { get; init; } = string.Empty;
    public string AzureContainer { get; init; } = string.Empty;
}
