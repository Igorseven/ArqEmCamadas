using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.EntitiesValidation;
using ArqEmCamadas.Domain.Interfaces;

namespace ArqEmCamadas.Api.IoC.Containers;

public static class ValidationContainer
{
    public static IServiceCollection AddValidationContainer(this IServiceCollection services) =>
        services.AddScoped<IValidate<User>, UserValidation>()
        ;
}