namespace EnterpriseMediaVault.Application.Abstractions;

public interface IAuditService
{
    Task WriteAsync(string action, string resourceType, string resourceId, string details, CancellationToken cancellationToken = default);
}
