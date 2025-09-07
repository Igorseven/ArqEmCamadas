using ArqEmCamadas.Api.Extensions;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Response;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Constants;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ArqEmCamadas.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class AuthenticationController(
    IAuthenticationCommandService authenticationCommandService)
    : ControllerBase
{
    
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitName.LimitingByIp)]
    [HttpPost("generate_access_token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationLoginResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<AuthenticationLoginResponse?> CreateAccessTokenAsync([FromBody] UserLoginRequest userLogin) =>
        authenticationCommandService.CreateAccessTokenAsync(userLogin);

    [AllowAnonymous]
    [EnableRateLimiting(RateLimitName.LimitingByIp)]
    [HttpPost("update_access_token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationLoginResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<AuthenticationLoginResponse?> CreateRefreshTokenAsync(
        [FromBody] UpdateAccessTokenRequest updateAccessToken) =>
        authenticationCommandService.CreateRefreshTokenAsync(
            updateAccessToken,
            updateAccessToken.AccessToken.GetUserIdFromToken());
    
}