using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Auth.Commands;

public sealed record RegisterUserCommand(string FullName, string Email, string Password, string RoleName)
    : IRequest<ApiResponse<AuthTokenDto>>;
