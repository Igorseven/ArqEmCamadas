using ArqEmCamadas.Domain.Constants;
using ArqEmCamadas.Domain.Providers;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class CorsSettings
{
    public static void AddCorsSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var frontUrls = configuration.GetSection(CorsConfigurationOptions.SectionName).Get<CorsConfigurationOptions>();
        
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName.CorsPolicy, builder =>
            {
                builder.WithMethods(frontUrls!.Methods)
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            });
        });
    }
}