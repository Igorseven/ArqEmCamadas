using System.Threading.RateLimiting;
using ArqEmCamadas.Domain.Constants;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class RateLimitingSettings
{
    private const int ToManyRequests = StatusCodes.Status429TooManyRequests;

    public static void AddRateLimitingSettings(this IServiceCollection services)
    {
        services.AddRateLimiter(config =>
        {
            config.AddPolicy(RateLimitName.LimitingByIp, httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(3)
                    })).RejectionStatusCode = ToManyRequests;
        });
        
        services.AddRateLimiter(config =>
        {
            config.AddPolicy(RateLimitName.LimitingByUser, httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(2)
                    })).RejectionStatusCode = ToManyRequests;
        });
    }
}