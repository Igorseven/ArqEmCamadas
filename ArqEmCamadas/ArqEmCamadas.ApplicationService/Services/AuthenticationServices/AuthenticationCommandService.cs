using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Response;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Traces;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Domain.Providers;
using Microsoft.Extensions.Options;

namespace ArqEmCamadas.ApplicationService.Services.AuthenticationServices;

public sealed class AuthenticationCommandService(
    ITokenManager tokenManager,
    IAuthenticationValidator validator,
    IUserFacadeQueryService userFacadeQueryService,
    INotificationHandler notificationHandler,
    IOptions<JwtTokenOptions> jwtTokenOptions)
    : IAuthenticationCommandService
{
    private readonly JwtTokenOptions _jwtTokenOptions = jwtTokenOptions.Value;

    public void Dispose() => tokenManager.Dispose();

    public async Task<AuthenticationLoginResponse?> CreateAccessTokenAsync(UserLoginRequest userLogin)
    {
        ArgumentNullException.ThrowIfNull(userLogin);

        var user = await userFacadeQueryService.FindUserByPredicateAsync(u => u.UserName == userLogin.Login);
        
        if (!validator.ValidateUserStatus(user)) 
            return null;
        
        if (!await validator.ValidateUserCredentialsAsync(userLogin))
            return null;

        await tokenManager.RevokeTokensAsync(
            userLogin.Login, 
            userLogin.SystemOrigin.ToString());

        var accessToken = await tokenManager.GenerateAccessTokenAsync(
            userLogin.Login,
            userLogin.SystemOrigin);
        
        var refreshToken = await tokenManager.GenerateRefreshTokenAsync(
            userLogin.Login, 
            userLogin.SystemOrigin,
            user!.Id);

        return new AuthenticationLoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes
        };
    }

    public async Task<AuthenticationLoginResponse?> CreateRefreshTokenAsync(
        UpdateAccessTokenRequest updateAccessToken,
        Guid userId)
    {

        if (userId == Guid.Empty) 
        {
            notificationHandler.CreateNotification(
                AuthenticationTrace.IdNotFound, 
                EMessage.NotFound.GetDescription().FormatTo(userId));
            
            return null;
        }

        var extractUserRequest = tokenManager.CreateExtractUserRequest(
            updateAccessToken, 
            userId);
        
        var userName = await userFacadeQueryService.ExtractUserFromAccessTokenAsync(extractUserRequest);

        if (!validator.ValidateRefreshTokenInput(userName, updateAccessToken.RefreshToken))
            return null;

        if (!await tokenManager.ValidateRefreshTokenAsync(userName!, updateAccessToken.RefreshToken))
        {
            notificationHandler.CreateNotification(
                AuthenticationTrace.RefreshToken, 
                EMessage.NotFound.GetDescription().FormatTo("Refresh Token"));
            
            return null;
        }

        await tokenManager.RevokeTokensAsync(
            userName!,
            updateAccessToken.SystemOrigin.ToString());

        var accessToken = await tokenManager.GenerateAccessTokenAsync(
            userName!,
            updateAccessToken.SystemOrigin);
        
        var refreshToken = await tokenManager.GenerateRefreshTokenAsync(
            userName!, 
            updateAccessToken.SystemOrigin, 
            userId);

        return new AuthenticationLoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes
        };
    }

    public Task UserSignOutAsync() => validator.SignOutAsync();
}