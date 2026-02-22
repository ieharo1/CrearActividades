using EnterpriseMediaVault.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EnterpriseMediaVault.Infrastructure;

public sealed class BootstrapHostedService(
    IServiceProvider provider,
    ILogger<BootstrapHostedService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = provider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        var indexes = scope.ServiceProvider.GetRequiredService<MongoIndexInitializer>();

        await indexes.EnsureIndexesAsync(cancellationToken);
        await seeder.SeedAsync(cancellationToken);

        logger.LogInformation("Mongo indexes and seed data ready.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
