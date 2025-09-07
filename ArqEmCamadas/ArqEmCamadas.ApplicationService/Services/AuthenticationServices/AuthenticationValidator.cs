using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Traces;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

namespace ArqEmCamadas.ApplicationService.Services.AuthenticationServices;

public sealed class AuthenticationValidator(
    IUserAuthenticationRepository userAuthenticationRepository,
    INotificationHandler notificationHandler)
    : IAuthenticationValidator
{

    public bool ValidateUserStatus(User? user)
    {
        if (user is null)
        {
            notificationHandler.CreateNotification(
                AuthenticationTrace.AccessOrRefreshToken, 
                AuthenticationTrace.LoginOrPassword);
            
            return false;
        }
        
        if (user.Status == EUserStatus.Active) 
            return true;
        
        notificationHandler.CreateNotification(
            AuthenticationTrace.AccessOrRefreshToken, 
            AuthenticationTrace.UserInactiveOrWithoutPermission);

        return false;
    }

    public async Task<bool> ValidateUserCredentialsAsync(UserLoginRequest userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Password))
        {
            return notificationHandler.CreateNotification(
                AuthenticationTrace.AccessToken,
                EMessage.Required.GetDescription().FormatTo("Senha"));
        }

        var result = await userAuthenticationRepository.UserAuthenticationAsync(
            userLogin.Login, 
            userLogin.Password);

        if (result.Succeeded) 
            return true;
        
        
        return notificationHandler.CreateNotification(
            AuthenticationTrace.AccessToken,
            result.SetNotificationBySignInResult());
    }

    public bool ValidateRefreshTokenInput(string? userName, string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return notificationHandler.CreateNotification(
                AuthenticationTrace.RefreshToken, 
                AuthenticationTrace.SecurityExtraction);
        }

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return notificationHandler.CreateNotification(
                AuthenticationTrace.RefreshToken, 
                EMessage.NotFound.GetDescription().FormatTo("Refresh Token"));
        }

        return true;
    }

    public Task SignOutAsync() => userAuthenticationRepository.UserSignOutAsync();
}