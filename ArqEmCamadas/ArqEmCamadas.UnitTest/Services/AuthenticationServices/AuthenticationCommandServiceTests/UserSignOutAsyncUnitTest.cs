using ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests.Base;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests;

public class UserSignOutAsyncUnitTest : AuthenticationCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task UserSignOutAsync_ValidSignOut_ShouldCompleteSuccessfully()
    {
        AuthenticationValidator
            .Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        await AuthenticationCommandService.UserSignOutAsync();

        AuthenticationValidator
            .Verify(x => x.SignOutAsync(), Times.Once);
        Assert.True(true);
    }
}
