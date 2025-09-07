using System.Text;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Api.Settings.Handlers;

public static class IdentitySettings
{
    public static void AddIdentitySettings(this IServiceCollection services)
    {

        services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.User.AllowedUserNameCharacters = EncodingExtension.GetAllWritableCharacters(Encoding.UTF8);
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
            }).AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
    }
}