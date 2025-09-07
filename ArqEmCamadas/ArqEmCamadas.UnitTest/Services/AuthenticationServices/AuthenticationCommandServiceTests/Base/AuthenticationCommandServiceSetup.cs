using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Services.AuthenticationServices;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Domain.Providers;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.Extensions.Options;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests.Base;

public abstract class AuthenticationCommandServiceSetup
{
    protected readonly Mock<IUserAuthenticationRepository> UserAuthenticationRepository;
    protected readonly Mock<IUserTokenRepository> UserTokenRepository;
    protected readonly Mock<IUserFacadeQueryService> UserFacadeQueryService;
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IOptions<JwtTokenOptions>> JwtTokenOptions;
    protected readonly Mock<ITokenManager> TokenManager;
    protected readonly Mock<IAuthenticationValidator> AuthenticationValidator;
    protected readonly AuthenticationCommandService AuthenticationCommandService;

    protected AuthenticationCommandServiceSetup()
    {
        UserAuthenticationRepository = new Mock<IUserAuthenticationRepository>();
        UserTokenRepository = new Mock<IUserTokenRepository>();
        UserFacadeQueryService = new Mock<IUserFacadeQueryService>();
        NotificationHandler = new Mock<INotificationHandler>();
        JwtTokenOptions = new Mock<IOptions<JwtTokenOptions>>();
        TokenManager = new Mock<ITokenManager>();
        AuthenticationValidator = new Mock<IAuthenticationValidator>();

        JwtTokenOptions
            .Setup(x => x.Value)
            .Returns(GetJwtTokenOptions());

        AuthenticationCommandService = new AuthenticationCommandService(
            TokenManager.Object,
            AuthenticationValidator.Object,
            UserFacadeQueryService.Object,
            NotificationHandler.Object,
            JwtTokenOptions.Object
        );
    }

    private static JwtTokenOptions GetJwtTokenOptions() =>
        new()
        {
            JwtKey = "valid-jwt-key-for-testing-purposes-only",
            Issuer = "Partilha.Api",
            Audience = "valid-audience",
            DurationInMinutes = 300,
            RequireHttpsMetadata = false
        };
}
