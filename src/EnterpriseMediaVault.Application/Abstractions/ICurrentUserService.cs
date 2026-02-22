namespace EnterpriseMediaVault.Application.Abstractions;

public interface ICurrentUserService
{
    string UserId { get; }
    string Role { get; }
    string TenantId { get; }
    bool IsAuthenticated { get; }
}
