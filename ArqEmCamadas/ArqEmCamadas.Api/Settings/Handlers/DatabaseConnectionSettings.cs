using ArqEmCamadas.Domain.Providers;
using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class DatabaseConnectionSettings
{
    public static void AddDatabaseConnectionSettings(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>((serviceProv, options) =>
            options.UseSqlServer(
                serviceProv.GetRequiredService<ConnectionStringOptions>().DefaultConnection, 
                sql => sql.CommandTimeout(180)));
    }
}