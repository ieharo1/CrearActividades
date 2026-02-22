namespace EnterpriseMediaVault.Application.DTOs;

public sealed class CreateUserDto
{
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
}
