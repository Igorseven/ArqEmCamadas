using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.Repository;

namespace ArqEmCamadas.Api.IoC.Containers;

public static class RepositoryContainer
{
      public static void AddRepositoryContainer(this IServiceCollection services)
      {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IDomainLoggerRepository, DomainLoggerRepository>();
        }
}