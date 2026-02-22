using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;

namespace EnterpriseMediaVault.Application.Features.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<ApiResponse<AuthTokenDto>>;
