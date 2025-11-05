using CafezinhoELivrosApi.Data;
using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CafezinhoELivrosAPI.Data.Seeds;

public class SeedHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeedHostedService> _logger;

    public SeedHostedService(IServiceProvider serviceProvider, ILogger<SeedHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔹 Iniciando Seed...");

        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            db.Database.Migrate();
            await SeedData.CreateRoles(roleManager, _logger);
            await SeedData.CreateAdmin(userManager, roleManager, _logger);
        }

        _logger.LogInformation("🔹 Seed finalizada!");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
