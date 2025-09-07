using ArqEmCamadas.Api.Extensions;
using ArqEmCamadas.Domain.Providers;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class ProviderSettings
{
    public static void AddProviderSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetConfigureOptions<ConnectionStringOptions>(configuration, ConnectionStringOptions.SectionName);
        services.SetConfigureOptions<EmailSettingsOptions>(configuration, EmailSettingsOptions.SectionName);
        services.SetConfigureOptions<CorsConfigurationOptions>(configuration, CorsConfigurationOptions.SectionName);
        services.SetConfigureOptions<JwtTokenOptions>(configuration, JwtTokenOptions.SectionName);
    }
}