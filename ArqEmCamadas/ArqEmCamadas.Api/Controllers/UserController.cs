using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Domain.UserPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArqEmCamadas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(
    IUserCommandService commandService,
    IUserQueryService queryService)
    : ControllerBase
{
    [Authorize(Roles = Policy.Owner)]
    [HttpPost("register_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<bool> Register([FromBody] UserRegisterRequest request) =>
        commandService.RegisterAsync(request, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}")]
    [HttpPut("update_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<bool> Update([FromBody] UserUpdateRequest request) =>
        commandService.UpdateAsync(request, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}")]
    [HttpPatch("activate_deactivate_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<bool> ActivateDeactivate([FromQuery] Guid analystId) =>
        commandService.ActivateOrDeactivateAsync(analystId, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}")]
    [HttpPatch("change_password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<bool> ChangePassword([FromBody] UserChangePasswordRequest request) =>
        commandService.ChangePasswordAsync(request, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}")]
    [HttpDelete("delete_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<bool> Delete([FromQuery] Guid analystId) =>
        commandService.DeleteAsync(analystId, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}, {Policy.Analyst}")]
    [HttpGet("find_user_by_id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAnalystResponse))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<UserSimpleResponse?> FindById([FromQuery] Guid userId) =>
        queryService.FindByIdAsync(userId, User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}, {Policy.Analyst}")]
    [HttpGet("find_user_by_token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAnalystResponse))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<UserSimpleResponse?> FindByToken() => 
        queryService.FindByIdAsync(User.GetUserId(), User.GetUserCredential());

    [Authorize(Roles = $"{Policy.Administrator}, {Policy.Owner}, {Policy.Analyst}")]
    [HttpGet("get_all_users_with_pagination")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageList<UserPaginationResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<PageList<UserPaginationResponse>> FindAllWithPagination([FromQuery] UserPageParams pageParams) =>
        queryService.FindAllWithPaginationAsync(pageParams, User.GetUserCredential());
}