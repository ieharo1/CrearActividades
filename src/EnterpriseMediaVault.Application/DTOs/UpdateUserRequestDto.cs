namespace EnterpriseMediaVault.Application.DTOs;

public sealed class UpdateUserRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Role { get; init; }
    public bool IsActive { get; init; } = true;
}
