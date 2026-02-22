using EnterpriseMediaVault.Application.Common;
using EnterpriseMediaVault.Application.DTOs;
using MediatR;
using EnterpriseMediaVault.Domain.Entities;
using EnterpriseMediaVault.Application.Abstractions;

namespace EnterpriseMediaVault.Application.Features.Users.Queries;

public sealed record GetAllUsersQuery() : IRequest<ApiResponse<List<UserDto>>>;

public sealed class GetAllUsersQueryHandler(
    IMongoRepository<User> users,
    IMongoRepository<Role> roles,
    ICurrentUserService currentUser)
    : IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserDto>>>
{
    public async Task<ApiResponse<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var allUsers = await users.FilterAsync(u => u.Where(x => !x.SoftDelete), ct);

        var allRoles = await roles.FilterAsync(r => r.Where(x => !x.SoftDelete), ct);
        var roleMap = allRoles.ToDictionary(r => r.Id, r => r.Name);

        var userDtos = allUsers.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            RoleId = u.RoleId,
            IsActive = u.IsActive,
            CreatedAtUtc = u.CreatedAtUtc,
            UpdatedAtUtc = u.UpdatedAtUtc
        }).ToList();

        return ApiResponse<List<UserDto>>.Ok(userDtos);
    }
}
