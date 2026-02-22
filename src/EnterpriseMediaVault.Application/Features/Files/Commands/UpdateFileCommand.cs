using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed record UpdateFileCommand(
    string FileId,
    string Name,
    Dictionary<string, object> Metadata) : IRequest<ApiResponse<FileDto>>;
