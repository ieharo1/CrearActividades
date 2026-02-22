using EnterpriseMediaVault.Application.Common;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed record RollbackFileVersionCommand(string FileId, int VersionNumber, string StorageStrategy)
    : IRequest<ApiResponse<bool>>;
