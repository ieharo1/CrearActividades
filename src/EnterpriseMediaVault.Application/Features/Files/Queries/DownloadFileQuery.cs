using EnterpriseMediaVault.Application.Common;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Queries;

public sealed record DownloadFileQuery(string FileId, string StorageStrategy)
    : IRequest<ApiResponse<DownloadFileResult>>;

public sealed class DownloadFileResult
{
    public string Name { get; init; } = string.Empty;
    public string MimeType { get; init; } = "application/octet-stream";
    public Stream Stream { get; init; } = Stream.Null;
}
