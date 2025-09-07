using ArqEmCamadas.Api.IoC.Containers;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Infra.ORM.Contexts;
using ArqEmCamadas.Infra.ORM.UoW;

namespace ArqEmCamadas.Api.IoC;

public static class InversionOfControlHandler
{
    public static void AddInversionOfControlHandler(this IServiceCollection services) =>
        services.AddScoped<ApplicationContext>()
            .AddScoped<INotificationHandler, NotificationHandler>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddServiceContainer()
            .AddPaginationContainer()
            .AddValidationContainer()
            .AddMapperContainer()
            .AddRepositoryContainer()
            ;
}