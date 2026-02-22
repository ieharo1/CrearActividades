namespace EnterpriseMediaVault.Application.DTOs;

public sealed class AuthTokenDto
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public UserInfo? User { get; init; }
}

public class UserInfo
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
