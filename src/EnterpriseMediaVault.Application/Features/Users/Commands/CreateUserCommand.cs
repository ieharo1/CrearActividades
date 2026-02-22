using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Users.Commands;

public sealed record CreateUserCommand(
    string Email,
    string FullName,
    string Password,
    string RoleId) : IRequest<ApiResponse<UserDto>>;
