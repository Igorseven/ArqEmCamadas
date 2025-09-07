using ArqEmCamadas.Api.Controllers;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using Moq;

namespace ArqEmCamadas.UnitTest.Controllers.UserControllerTests.Base;

public abstract class UserControllerSetup
{
    protected readonly Mock<IUserCommandService> UserCommandService;
    protected readonly Mock<IUserQueryService> UserQueryService;
    protected readonly UserController UserController;

    protected UserControllerSetup()
    {
        UserCommandService = new Mock<IUserCommandService>();
        UserQueryService = new Mock<IUserQueryService>();

        UserController = new UserController(
            UserCommandService.Object,
            UserQueryService.Object);
    }
}