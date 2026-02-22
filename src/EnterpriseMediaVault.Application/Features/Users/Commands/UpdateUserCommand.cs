using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Users.Commands;

public sealed record UpdateUserCommand(
    string UserId,
    string Email,
    string FullName,
    string? Password,
    string RoleId,
    bool IsActive) : IRequest<ApiResponse<UserDto>>;
