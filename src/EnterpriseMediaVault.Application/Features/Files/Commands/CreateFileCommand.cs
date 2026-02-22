using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed record CreateFileCommand(
    string Name,
    string MimeType,
    long Size,
    string FolderId,
    Dictionary<string, object> Metadata) : IRequest<ApiResponse<FileDto>>;
