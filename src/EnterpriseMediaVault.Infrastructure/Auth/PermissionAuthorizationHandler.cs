using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseMediaVault.Infrastructure.Auth;

public sealed class PermissionAuthorizationHandler(
    IMongoRepository<Permission> permissions,
    ICurrentUserService currentUser)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!currentUser.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        var userPermissions = await permissions.FilterAsync(
            q => q.Where(p => !p.SoftDelete
                && p.TenantId == currentUser.TenantId
                && p.SubjectType == "User"
                && p.SubjectId == currentUser.UserId
                && p.Action == requirement.Action
                && p.ResourceType == requirement.ResourceType
                && p.Allowed));

        if (userPermissions.Count > 0)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
