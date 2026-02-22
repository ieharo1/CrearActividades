using Microsoft.AspNetCore.Authorization;

namespace EnterpriseMediaVault.Infrastructure.Auth;

public sealed class PermissionRequirement(string action, string resourceType) : IAuthorizationRequirement
{
    public string Action { get; } = action;
    public string ResourceType { get; } = resourceType;
}
