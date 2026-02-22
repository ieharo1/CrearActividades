using System.Security.Claims;
using EnterpriseMediaVault.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace EnterpriseMediaVault.Infrastructure.Auth;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string UserId => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? accessor.HttpContext?.User.FindFirstValue("sub")
        ?? "system";

    public string Role => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "System";

    public string TenantId => accessor.HttpContext?.User.FindFirstValue("tenant") ?? "default";

    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
