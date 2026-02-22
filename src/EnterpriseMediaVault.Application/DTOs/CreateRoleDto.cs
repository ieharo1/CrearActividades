namespace EnterpriseMediaVault.Application.DTOs;

public sealed class CreateRoleDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
