using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.ApplicationService.Mappers;

namespace ArqEmCamadas.Api.IoC.Containers;

public static class MapperContainer
{
    public static IServiceCollection AddMapperContainer(this IServiceCollection services)
    {
        return services
            .AddTransient<IUserMapper, UserMapper>()
            ;
    }
}