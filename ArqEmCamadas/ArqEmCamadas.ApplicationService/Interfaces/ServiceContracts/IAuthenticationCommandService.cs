using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Response;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IAuthenticationCommandService : IDisposable
{
    Task<AuthenticationLoginResponse?> CreateAccessTokenAsync(UserLoginRequest userLogin);
    
    Task<AuthenticationLoginResponse?> CreateRefreshTokenAsync(
        UpdateAccessTokenRequest updateAccessToken,
        Guid userId);
    
    Task UserSignOutAsync();
}