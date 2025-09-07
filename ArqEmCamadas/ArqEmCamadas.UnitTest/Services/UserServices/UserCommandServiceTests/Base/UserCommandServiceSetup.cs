using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Services.UserServices;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.ValidationSettings;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;

public abstract class UserCommandServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<User>> UserValidator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IUserRepository> UserRepository;
    protected readonly Mock<IUserMapper> UserMapper;
    protected readonly Mock<IRoleFacadeQueryService> RoleFacadeQueryService;
    protected readonly Mock<IEmailCommandService> EmailCommandService;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;
    protected readonly UserCommandService UserCommandService;

    protected UserCommandServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        UserValidator = new Mock<IValidate<User>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        UserRepository = new Mock<IUserRepository>();
        UserMapper = new Mock<IUserMapper>();
        RoleFacadeQueryService = new Mock<IRoleFacadeQueryService>();
        EmailCommandService = new Mock<IEmailCommandService>();
        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);

        UserCommandService = new UserCommandService(
            NotificationHandler.Object,
            UserValidator.Object,
            LoggerHandler.Object,
            UserRepository.Object,
            UserMapper.Object,
            RoleFacadeQueryService.Object,
            EmailCommandService.Object
        );
    }

    protected void CreateNotification() =>
        Errors.Add("Error", "Error");
}