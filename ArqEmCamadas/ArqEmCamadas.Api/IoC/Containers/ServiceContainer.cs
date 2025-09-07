using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Services.AuthenticationServices;
using ArqEmCamadas.ApplicationService.Services.EmailServices;
using ArqEmCamadas.ApplicationService.Services.LoggerHandlerServices;
using ArqEmCamadas.ApplicationService.Services.RoleServices;
using ArqEmCamadas.ApplicationService.Services.UserServices;
using ArqEmCamadas.Domain.Interfaces;

namespace ArqEmCamadas.Api.IoC.Containers;

public static class ServiceContainer
{
    public static IServiceCollection AddServiceContainer(this IServiceCollection services) =>
        services.AddScoped<IUserCommandService, UserCommandService>()
            .AddScoped<IUserFacadeCommandService, UserCommandService>()
            .AddScoped<IUserQueryService, UserQueryService>()
            .AddScoped<IRoleFacadeQueryService, RoleFacadeQueryService>()
            .AddScoped<IEmailCommandService, EmailCommandService>()
            .AddScoped<IUserFacadeQueryService, UserFacadeQueryService>()
            .AddScoped<ITokenManager, TokenManager>()
            .AddScoped<IAuthenticationValidator, AuthenticationValidator>()
            .AddScoped<IAuthenticationCommandService, AuthenticationCommandService>()
            .AddScoped<ILoggerHandler, LoggerHandler>()
        ;
}