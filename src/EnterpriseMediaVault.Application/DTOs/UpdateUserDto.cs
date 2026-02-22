namespace EnterpriseMediaVault.Application.DTOs;

public sealed class UpdateUserDto
{
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Password { get; init; }
    public string RoleId { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
