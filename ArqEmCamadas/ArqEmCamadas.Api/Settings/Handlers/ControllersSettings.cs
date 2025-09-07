using System.Text.Json.Serialization;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class ControllersSettings
{
    public static void AddControllersSettings(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

    }
}