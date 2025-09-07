using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests;

public class CreateAccessTokenAsyncUnitTest : AuthenticationCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task CreateAccessTokenAsync_ValidUser_ReturnAuthenticationLoginResponse()
    {
        // Arrange
        var userLogin = new UserLoginRequest
        {
            Login = "testuser",
            Password = "testpassword",
            SystemOrigin = Guid.NewGuid()
        };

        var user = UserBuilderTest.NewObject().Build();

        UserFacadeQueryService
            .Setup(x => x.FindUserByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        AuthenticationValidator
            .Setup(x => x.ValidateUserStatus(user))
            .Returns(true);
        
        AuthenticationValidator
            .Setup(x => x.ValidateUserCredentialsAsync(userLogin))
            .ReturnsAsync(true);
        
        TokenManager
            .Setup(x => x.RevokeTokensAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        
        TokenManager
            .Setup(x => x.GenerateAccessTokenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync("test-access-token");
        
        TokenManager
            .Setup(x => x.GenerateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync("test-refresh-token");

        var result = await AuthenticationCommandService.CreateAccessTokenAsync(userLogin);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(300, result.Expiry);
    }

    [Fact]
    [Trait("Command", "User not found")]
    public async Task CreateAccessTokenAsync_UserNotFound_ReturnNull()
    {
        var userLogin = new UserLoginRequest
        {
            Login = "nonexistentuser",
            Password = "testpassword",
            SystemOrigin = Guid.NewGuid()
        };

        UserFacadeQueryService
            .Setup(x => x.FindUserByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);
        
        AuthenticationValidator
            .Setup(x => x.ValidateUserStatus(null))
            .Returns(false);

        var result = await AuthenticationCommandService.CreateAccessTokenAsync(userLogin);

        Assert.Null(result);
        UserFacadeQueryService
            .Verify(x => x.FindUserByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),    
                It.IsAny<bool>()), Times.Once);
        AuthenticationValidator
            .Verify(x => x.ValidateUserCredentialsAsync(It.IsAny<UserLoginRequest>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Authentication fails")]
    public async Task CreateAccessTokenAsync_AuthenticationFails_ReturnNull()
    {
        var userLogin = new UserLoginRequest
        {
            Login = "testuser",
            Password = "wrongpassword",
            SystemOrigin = Guid.NewGuid()
        };

        var user = UserBuilderTest.NewObject().Build();

        UserFacadeQueryService
            .Setup(x => x.FindUserByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        AuthenticationValidator
            .Setup(x => x.ValidateUserStatus(user))
            .Returns(true);
        
        AuthenticationValidator
            .Setup(x => x.ValidateUserCredentialsAsync(userLogin))
            .ReturnsAsync(false);

        var result = await AuthenticationCommandService.CreateAccessTokenAsync(userLogin);

        Assert.Null(result);
        UserFacadeQueryService
            .Verify(x => x.FindUserByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        AuthenticationValidator
            .Verify(x => x.ValidateUserCredentialsAsync(userLogin), Times.Once);
        TokenManager
            .Verify(x => x.RevokeTokensAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
