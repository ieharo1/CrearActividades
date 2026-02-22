using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed record UploadFileCommand(
    string FileName,
    string MimeType,
    long Size,
    string FolderId,
    Stream FileStream,
    Dictionary<string, object> Metadata,
    string StorageStrategy)
    : IRequest<ApiResponse<FileDto>>;
