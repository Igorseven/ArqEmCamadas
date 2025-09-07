using ArqEmCamadas.Infra.Interfaces.ServiceContracts;
using ArqEmCamadas.Infra.Services;

namespace ArqEmCamadas.Api.IoC.Containers;

public static class PaginationContainer
{
    public static IServiceCollection AddPaginationContainer(this IServiceCollection services) =>
        services.AddScoped(typeof(IPaginationQueryService<>), typeof(PaginationQueryService<>));
}