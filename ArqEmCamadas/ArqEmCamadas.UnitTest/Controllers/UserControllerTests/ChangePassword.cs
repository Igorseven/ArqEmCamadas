using ArqEmCamadas.Api.Controllers;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.Domain.UserPolicies;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Controllers.UserControllerTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ArqEmCamadas.UnitTest.Controllers.UserControllerTests;

public class ChangePassword : UserControllerSetup
{
    public static IEnumerable<object[]> ValidRoles()
    {
        yield return [Policy.Administrator];
        yield return [Policy.Owner];
    }

    [Theory]
    [Trait("PATCH", "Return Status Code 200 Ok")]
    [MemberData(nameof(ValidRoles))]
    public async Task ChangePassword_ValidationRoles_ReturnStatusCodeOk(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("ChangePassword");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoChangePassword = new UserChangePasswordRequest()
        {
            UserId = Guid.NewGuid(),
            NewPassword = "NewSecurePassword123!"
        };

        UserCommandService
            .Setup(s => s.ChangePasswordAsync(
                It.IsAny<UserChangePasswordRequest>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(true);

        var result = await UserController.ChangePassword(dtoChangePassword);

        var status = UserController.Response.StatusCode;

        Assert.True(result);
        Assert.Equal(statusOk, status);

        UserCommandService
            .Verify(s => s.ChangePasswordAsync(
                It.IsAny<UserChangePasswordRequest>(),
                It.IsAny<UserCredential>()), Times.Once);
    }

    public static IEnumerable<object[]> InvalidRoles()
    {
        yield return [Policy.Analyst];
        yield return [Policy.System];
    }

    [Theory]
    [Trait("PATCH", "Return Status Code 403 Forbidden")]
    [MemberData(nameof(InvalidRoles))]
    public async Task ChangePassword_ValidationRoles_ReturnStatusCodeForbidden(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("ChangePassword");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoChangePassword = new UserChangePasswordRequest()
        {
            UserId = Guid.NewGuid(),
            NewPassword = "NewSecurePassword123!"
        };

        UserCommandService
            .Setup(s => s.ChangePasswordAsync(
                It.IsAny<UserChangePasswordRequest>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(true);

        var result = await UserController.ChangePassword(dtoChangePassword);

        var status = UserController.Response.StatusCode;

        Assert.True(result);
        Assert.Equal(statusForbidden, status);

        UserCommandService
            .Verify(s => s.ChangePasswordAsync(
                It.IsAny<UserChangePasswordRequest>(),
                It.IsAny<UserCredential>()), Times.Once);
    }
}
