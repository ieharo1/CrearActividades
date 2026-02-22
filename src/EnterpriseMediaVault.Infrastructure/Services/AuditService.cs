using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EnterpriseMediaVault.Infrastructure.Services;

public sealed class AuditService(
    IMongoRepository<AuditLog> auditLogs,
    ICurrentUserService currentUser,
    IHttpContextAccessor accessor)
    : IAuditService
{
    public async Task WriteAsync(string action, string resourceType, string resourceId, string details, CancellationToken cancellationToken = default)
    {
        await auditLogs.InsertAsync(new AuditLog
        {
            UserId = currentUser.UserId,
            Action = action,
            ResourceType = resourceType,
            ResourceId = resourceId,
            Details = details,
            IpAddress = accessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            TenantId = currentUser.TenantId
        }, cancellationToken);
    }
}
