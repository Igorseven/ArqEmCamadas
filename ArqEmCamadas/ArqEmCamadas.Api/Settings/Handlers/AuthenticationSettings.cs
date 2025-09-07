using System.Text;
using ArqEmCamadas.Domain.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class AuthenticationSettings
{
    public static void AddAuthenticationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtProvider = configuration.GetSection(JwtTokenOptions.SectionName).Get<JwtTokenOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = jwtProvider!.RequireHttpsMetadata;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = jwtProvider.Audience,
                    ValidIssuer = jwtProvider.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtProvider.JwtKey))
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme);
    }
}