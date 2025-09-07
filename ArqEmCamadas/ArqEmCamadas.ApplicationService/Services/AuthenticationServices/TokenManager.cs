using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Providers;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.IdentityModel.Tokens;

namespace ArqEmCamadas.ApplicationService.Services.AuthenticationServices;

public sealed class TokenManager : ITokenManager
{
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUserFacadeQueryService _userFacadeQueryService;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly SymmetricSecurityKey _key;
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    public TokenManager(
        IUserTokenRepository userTokenRepository,
        IUserFacadeQueryService userFacadeQueryService,
        JwtTokenOptions jwtTokenOptions)
    {
        _userTokenRepository = userTokenRepository;
        _userFacadeQueryService = userFacadeQueryService;
        _jwtTokenOptions = jwtTokenOptions;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.JwtKey));
    }
    
    public void Dispose() => _userTokenRepository.Dispose();

    public async Task<string> GenerateAccessTokenAsync(
        string userName,
        Guid systemOrigin)
    {
        var claims = await _userFacadeQueryService.FindAllUserRolesAndClaimsAsync(userName, systemOrigin);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtTokenOptions.DurationInMinutes),
            IssuedAt = DateTime.UtcNow,
            Issuer = _jwtTokenOptions.Issuer,
            Audience = systemOrigin.ToString(),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithm)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(
        string userName, 
        Guid systemOrigin,
        Guid userId)
    {
        using var randomGenerator = RandomNumberGenerator.Create();
        var randomNumbers = new byte[32];
        randomGenerator.GetBytes(randomNumbers);

        var token = Convert.ToBase64String(randomNumbers);

        var refreshToken = new UserToken
        {
            UserId = userId,
            Name = userName,
            LoginProvider = systemOrigin.ToString(),
            Value = token
        };

        await _userTokenRepository.SaveAsync(refreshToken);
        
        return token;
    }

    public async Task RevokeTokensAsync(
        string userName,
        string loginProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(loginProvider);

        await _userTokenRepository.DeleteAsync(
            r => r.Name == userName && r.LoginProvider == loginProvider);
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userName, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        return await _userTokenRepository.ExistsByPredicateAsync(
            r => r.Name == userName && r.Value == refreshToken);
    }

    public TokenValidationParameters CreateTokenValidationParameters(Guid systemOrigin)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = systemOrigin.ToString(),
            ValidIssuer = _jwtTokenOptions.Issuer,
            IssuerSigningKey = _key,
        };
    }

    public ExtractUserRequest CreateExtractUserRequest(UpdateAccessTokenRequest updateAccessToken, Guid userId)
    {
        return new ExtractUserRequest
        {
            UserId = userId,
            AccessToken = updateAccessToken.AccessToken,
            SecurityAlgorithm = SecurityAlgorithm,
            TokenValidationParameters = CreateTokenValidationParameters(updateAccessToken.SystemOrigin)
        };
    }
}