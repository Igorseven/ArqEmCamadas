using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IAuthenticationValidator
{
    bool ValidateUserStatus(User? user);
    Task<bool> ValidateUserCredentialsAsync(UserLoginRequest userLogin);
    bool ValidateRefreshTokenInput(string? userName, string? refreshToken);
    Task SignOutAsync();
}