using EnterpriseMediaVault.Application.Common;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Files.Commands;

public sealed record DeleteFileCommand(string FileId) : IRequest<ApiResponse<bool>>;
