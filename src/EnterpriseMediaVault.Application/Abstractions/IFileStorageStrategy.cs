namespace EnterpriseMediaVault.Application.Abstractions;

public interface IFileStorageStrategy
{
    string Name { get; }
    Task<string> SaveAsync(string fileName, string contentType, Stream stream, Dictionary<string, object> metadata, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadAsync(string storageReference, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storageReference, CancellationToken cancellationToken = default);
}
