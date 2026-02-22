using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Domain.Entities;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Permissions.Commands;

public sealed class GrantPermissionCommandHandler(
    IMongoRepository<Permission> permissions,
    ICurrentUserService currentUser,
    IAuditService auditService)
    : IRequestHandler<GrantPermissionCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(GrantPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = new Permission
        {
            SubjectId = request.SubjectId,
            SubjectType = request.SubjectType,
            ResourceId = request.ResourceId,
            ResourceType = request.ResourceType,
            Action = request.Action,
            Allowed = request.Allowed,
            TenantId = currentUser.TenantId
        };

        await permissions.InsertAsync(permission, cancellationToken);
        await auditService.WriteAsync("GRANT_PERMISSION", "Permission", permission.Id, $"{request.Action}:{request.ResourceType}", cancellationToken);

        return ApiResponse<bool>.Ok(true, "Permiso asignado");
    }
}
