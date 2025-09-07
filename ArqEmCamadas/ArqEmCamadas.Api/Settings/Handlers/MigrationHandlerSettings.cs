using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class MigrationHandlerSettings
{
    public static async Task MigrateDatabaseAsync(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        await using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        using var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        await appContext.Database.MigrateAsync();
        
        var seedHandler = new SeedHandler(appContext, userRepository);

        await seedHandler.Seed();
    }
}