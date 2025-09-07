using ArqEmCamadas.Api.Filters;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class FiltersSettings
{
    public static void AddFiltersSettings(this IServiceCollection services)
    {
        services.AddMvc(config => config.Filters.AddService<NotificationFilter>());
        services.AddMvc(config => config.Filters.AddService<UnitOfWorkFilter>());
        services.AddMvc(config => config.Filters.AddService<LoggerFilter>());

        services.AddScoped<NotificationFilter>();
        services.AddScoped<UnitOfWorkFilter>();
        services.AddScoped<LoggerFilter>();
    }
}