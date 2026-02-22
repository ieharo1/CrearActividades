using System.Text;
using EnterpriseMediaVault.Application.Abstractions;
using EnterpriseMediaVault.Infrastructure.Auth;
using EnterpriseMediaVault.Infrastructure.Configuration;
using EnterpriseMediaVault.Infrastructure.Mongo;
using EnterpriseMediaVault.Infrastructure.Persistence;
using EnterpriseMediaVault.Infrastructure.Repositories;
using EnterpriseMediaVault.Infrastructure.Services;
using EnterpriseMediaVault.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EnterpriseMediaVault.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<StorageSettings>(configuration.GetSection(StorageSettings.SectionName));

        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
        services.AddScoped<IAuditService, AuditService>();

        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IFileStorageStrategy, LocalFileStorageStrategy>();
        services.AddScoped<IFileStorageStrategy, GridFsStorageStrategy>();
        services.AddScoped<IFileStorageStrategy, S3StorageStrategy>();
        services.AddScoped<IFileStorageStrategy, AzureBlobStorageStrategy>();
        services.AddScoped<IStorageStrategyResolver, StorageStrategyResolver>();

        services.AddScoped<DbSeeder>();
        services.AddScoped<MongoIndexInitializer>();
        services.AddHostedService<BootstrapHostedService>();

        var jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = key
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            options.AddPolicy("ManagersOrAdmin", p => p.RequireRole("Admin", "Manager"));
            options.AddPolicy("AuditorRead", p => p.RequireRole("Admin", "Manager", "Auditor"));
        });
        services.AddSingleton<IAuthorizationPolicyProvider, DynamicPermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
