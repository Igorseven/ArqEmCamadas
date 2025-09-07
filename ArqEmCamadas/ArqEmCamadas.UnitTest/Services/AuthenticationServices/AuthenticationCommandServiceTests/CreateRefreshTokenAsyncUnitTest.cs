using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests.Base;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.AuthenticationServices.AuthenticationCommandServiceTests;

public class CreateRefreshTokenAsyncUnitTest : AuthenticationCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task CreateRefreshTokenAsync_ValidRefreshToken_ReturnAuthenticationLoginResponse()
    {
        // Arrange
        var updateAccessToken = new UpdateAccessTokenRequest
        {
            RefreshToken = "valid-refresh-token",
            SystemOrigin = Guid.NewGuid(),
            AccessToken = "valid-access-token"
        };

        var userId = Guid.NewGuid();
        var userName = "testuser";

        var extractUserRequest = new ExtractUserRequest
        {
            UserId = userId,
            AccessToken = updateAccessToken.AccessToken,
            SecurityAlgorithm = "HS256",
            TokenValidationParameters = new TokenValidationParameters()
        };
        
        TokenManager
            .Setup(x => x.CreateExtractUserRequest(updateAccessToken, userId))
            .Returns(extractUserRequest);
            
        UserFacadeQueryService
            .Setup(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()))
            .ReturnsAsync(userName);
            
        AuthenticationValidator
            .Setup(x => x.ValidateRefreshTokenInput(userName, updateAccessToken.RefreshToken))
            .Returns(true);

        TokenManager
            .Setup(x => x.ValidateRefreshTokenAsync(userName, updateAccessToken.RefreshToken))
            .ReturnsAsync(true);
            
        TokenManager
            .Setup(x => x.RevokeTokensAsync(userName, updateAccessToken.SystemOrigin.ToString()))
            .Returns(Task.CompletedTask);
        
        TokenManager
            .Setup(x => x.GenerateAccessTokenAsync(userName, updateAccessToken.SystemOrigin))
            .ReturnsAsync("new-access-token");
        
        TokenManager
            .Setup(x => x.GenerateRefreshTokenAsync(userName, updateAccessToken.SystemOrigin, userId))
            .ReturnsAsync("new-refresh-token");

        var result = await AuthenticationCommandService.CreateRefreshTokenAsync(updateAccessToken, userId);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(300, result.Expiry);
        UserFacadeQueryService
            .Verify(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once);
        AuthenticationValidator
            .Verify(x => x.ValidateRefreshTokenInput(userName, updateAccessToken.RefreshToken), Times.Once);
        TokenManager
            .Verify(x => x.ValidateRefreshTokenAsync(userName, updateAccessToken.RefreshToken), Times.Once);
    }

    [Fact]
    [Trait("Command", "User extraction fails")]
    public async Task CreateRefreshTokenAsync_UserExtractionFails_ReturnNull()
    {
        var updateAccessToken = new UpdateAccessTokenRequest
        {
            RefreshToken = "valid-refresh-token",
            SystemOrigin = Guid.NewGuid(),
            AccessToken = "valid-access-token"
        };

        var userId = Guid.NewGuid();

        var extractUserRequest = new ExtractUserRequest
        {
            UserId = userId,
            AccessToken = updateAccessToken.AccessToken,
            SecurityAlgorithm = "HS256",
            TokenValidationParameters = new TokenValidationParameters()
        };
        
        TokenManager
            .Setup(x => x.CreateExtractUserRequest(updateAccessToken, userId))
            .Returns(extractUserRequest);
            
        UserFacadeQueryService
            .Setup(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()))
            .ReturnsAsync((string?)null);
            
        AuthenticationValidator
            .Setup(x => x.ValidateRefreshTokenInput(null, updateAccessToken.RefreshToken))
            .Returns(false);

        var result = await AuthenticationCommandService.CreateRefreshTokenAsync(updateAccessToken, userId);

        Assert.Null(result);
        UserFacadeQueryService
            .Verify(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once);
        AuthenticationValidator
            .Verify(x => x.ValidateRefreshTokenInput(null, updateAccessToken.RefreshToken), Times.Once);
        TokenManager
            .Verify(x => x.ValidateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Refresh token not found")]
    public async Task CreateRefreshTokenAsync_RefreshTokenNotFound_ReturnNull()
    {
        var updateAccessToken = new UpdateAccessTokenRequest
        {
            RefreshToken = "invalid-refresh-token",
            SystemOrigin = Guid.NewGuid(),
            AccessToken = "valid-access-token"
        };

        var userId = Guid.NewGuid();
        var userName = "testuser";

        var extractUserRequest = new ExtractUserRequest
        {
            UserId = userId,
            AccessToken = updateAccessToken.AccessToken,
            SecurityAlgorithm = "HS256",
            TokenValidationParameters = new TokenValidationParameters()
        };
        
        TokenManager
            .Setup(x => x.CreateExtractUserRequest(updateAccessToken, userId))
            .Returns(extractUserRequest);
            
        UserFacadeQueryService
            .Setup(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()))
            .ReturnsAsync(userName);
            
        AuthenticationValidator
            .Setup(x => x.ValidateRefreshTokenInput(userName, updateAccessToken.RefreshToken))
            .Returns(true);

        TokenManager
            .Setup(x => x.ValidateRefreshTokenAsync(userName, updateAccessToken.RefreshToken))
            .ReturnsAsync(false);

        var result = await AuthenticationCommandService.CreateRefreshTokenAsync(updateAccessToken, userId);

        Assert.Null(result);
        UserFacadeQueryService
            .Verify(x => x.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once);
        AuthenticationValidator
            .Verify(x => x.ValidateRefreshTokenInput(userName, updateAccessToken.RefreshToken), Times.Once);
        TokenManager
            .Verify(x => x.ValidateRefreshTokenAsync(userName, updateAccessToken.RefreshToken), Times.Once);
        NotificationHandler
            .Verify(x => x.CreateNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
