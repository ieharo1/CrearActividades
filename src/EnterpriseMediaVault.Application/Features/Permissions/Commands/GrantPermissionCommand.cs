using EnterpriseMediaVault.Application.Common;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Permissions.Commands;

public sealed record GrantPermissionCommand(
    string SubjectId,
    string SubjectType,
    string ResourceId,
    string ResourceType,
    string Action,
    bool Allowed = true) : IRequest<ApiResponse<bool>>;
