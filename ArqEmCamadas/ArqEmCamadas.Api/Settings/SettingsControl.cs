using ArqEmCamadas.Api.Settings.Handlers;

namespace ArqEmCamadas.Api.Settings;

public static class SettingsControl
{
    public static void AddSettingsControl(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProviderSettings(configuration);
        services.AddControllersSettings();
        services.AddCorsSettings(configuration);
        services.AddDatabaseConnectionSettings();
        services.AddIdentitySettings();
        services.AddAuthenticationSettings(configuration);
        services.AddFiltersSettings();
        services.AddSwaggerSettings();
        services.AddRateLimitingSettings();
    }
}