using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using Microsoft.IdentityModel.Tokens;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface ITokenManager : IDisposable
{
    Task<string> GenerateAccessTokenAsync(string userName, Guid systemOrigin);
    Task<string> GenerateRefreshTokenAsync(string userName, Guid systemOrigin, Guid userId);
    Task RevokeTokensAsync(string userName, string loginProvider);
    Task<bool> ValidateRefreshTokenAsync(string userName, string refreshToken);
    TokenValidationParameters CreateTokenValidationParameters(Guid systemOrigin);
    ExtractUserRequest CreateExtractUserRequest(UpdateAccessTokenRequest updateAccessToken, Guid userId);
}