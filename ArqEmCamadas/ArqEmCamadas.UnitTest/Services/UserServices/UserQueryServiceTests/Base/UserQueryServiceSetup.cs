using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Services.UserServices;
using ArqEmCamadas.Domain.Providers;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.Extensions.Options;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserQueryServiceTests.Base;

public abstract class UserQueryServiceSetup
{
    protected readonly Mock<IUserRepository> UserRepository;
    protected readonly Mock<IUserMapper> UserMapper;
    protected readonly Mock<IRoleFacadeQueryService> RoleFacadeQueryService;
    protected readonly Mock<IOptions<JwtTokenOptions>> JwtTokenOptions;
    protected UserQueryService UserQueryService;

    protected UserQueryServiceSetup()
    {
        UserRepository = new Mock<IUserRepository>();
        UserMapper = new Mock<IUserMapper>();
        RoleFacadeQueryService = new Mock<IRoleFacadeQueryService>();
        JwtTokenOptions = new Mock<IOptions<JwtTokenOptions>>();

        UserQueryService = new UserQueryService(
            UserRepository.Object,
            UserMapper.Object
        );

        JwtTokenOptions
            .Setup(x => x.Value)
            .Returns(GetJwtTokenOptions());
    }

    private static JwtTokenOptions GetJwtTokenOptions() =>
        new()
        {
            JwtKey = "valid-jwt-key",
            Issuer = "Partilha.Api",
            Audience = "valid-audience",
            DurationInMinutes = 300,
            RequireHttpsMetadata = false
        };
}