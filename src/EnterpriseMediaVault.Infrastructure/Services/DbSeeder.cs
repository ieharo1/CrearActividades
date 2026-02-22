using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Domain.Entities;

namespace EnterpriseMediaVault.Infrastructure.Services;

public sealed class DbSeeder(
    IMongoRepository<Role> roles,
    IMongoRepository<User> users,
    IPasswordHasher hasher)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var roleCount = await roles.CountAsync(cancellationToken: cancellationToken);
        if (roleCount == 0)
        {
            await roles.InsertAsync(new Role { Name = "Admin", Description = "Full access" }, cancellationToken);
            await roles.InsertAsync(new Role { Name = "Manager", Description = "Team management" }, cancellationToken);
            await roles.InsertAsync(new Role { Name = "Employee", Description = "Standard operations" }, cancellationToken);
            await roles.InsertAsync(new Role { Name = "Auditor", Description = "Read and audit" }, cancellationToken);
        }

        var allRoles = await roles.FilterAsync(q => q.Where(r => !r.SoftDelete), cancellationToken);
        var roleMap = allRoles.ToDictionary(r => r.Name, r => r.Id);

        var adminExists = (await users.FilterAsync(q => q.Where(u => u.Email == "admin@vault.local" && !u.SoftDelete), cancellationToken)).Any();
        if (!adminExists && roleMap.ContainsKey("Admin"))
        {
            await users.InsertAsync(new User
            {
                FullName = "System Admin",
                Email = "admin@vault.local",
                PasswordHash = hasher.Hash("Admin12345!"),
                RoleId = roleMap["Admin"]
            }, cancellationToken);
        }

        var managerExists = (await users.FilterAsync(q => q.Where(u => u.Email == "manager@vault.local" && !u.SoftDelete), cancellationToken)).Any();
        if (!managerExists && roleMap.ContainsKey("Manager"))
        {
            await users.InsertAsync(new User
            {
                FullName = "Manager User",
                Email = "manager@vault.local",
                PasswordHash = hasher.Hash("Manager123!"),
                RoleId = roleMap["Manager"]
            }, cancellationToken);
        }

        var employeeExists = (await users.FilterAsync(q => q.Where(u => u.Email == "employee@vault.local" && !u.SoftDelete), cancellationToken)).Any();
        if (!employeeExists && roleMap.ContainsKey("Employee"))
        {
            await users.InsertAsync(new User
            {
                FullName = "Employee User",
                Email = "employee@vault.local",
                PasswordHash = hasher.Hash("Employee123!"),
                RoleId = roleMap["Employee"]
            }, cancellationToken);
        }

        var auditorExists = (await users.FilterAsync(q => q.Where(u => u.Email == "auditor@vault.local" && !u.SoftDelete), cancellationToken)).Any();
        if (!auditorExists && roleMap.ContainsKey("Auditor"))
        {
            await users.InsertAsync(new User
            {
                FullName = "Auditor User",
                Email = "auditor@vault.local",
                PasswordHash = hasher.Hash("Auditor123!"),
                RoleId = roleMap["Auditor"]
            }, cancellationToken);
        }
    }
}
