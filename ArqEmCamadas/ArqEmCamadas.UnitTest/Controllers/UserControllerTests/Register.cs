using ArqEmCamadas.Api.Controllers;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.Domain.UserPolicies;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Controllers.UserControllerTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ArqEmCamadas.UnitTest.Controllers.UserControllerTests;

public class Register : UserControllerSetup
{
    public static IEnumerable<object[]> ValidRoles()
    {
        yield return [Policy.Owner];
    }

    [Theory]
    [Trait("POST", "Return Status Code 200 Ok")]
    [MemberData(nameof(ValidRoles))]
    public async Task Register_ValidationRoles_ReturnStatusCodeOk(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("Register");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoRegister = new UserRegisterRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "@Tester1256"
        };

        UserCommandService
            .Setup(s => s.RegisterAsync(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(true);

        var result = await UserController.Register(dtoRegister);

        var status = UserController.Response.StatusCode;

        Assert.True(result);
        Assert.Equal(statusOk, status);

        UserCommandService
            .Verify(s => s.RegisterAsync(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<UserCredential>()), Times.Once);
    }

    public static IEnumerable<object[]> InvalidRoles()
    {
        yield return [Policy.Administrator];
        yield return [Policy.Analyst];
    }

    [Theory]
    [Trait("POST", "Return Status Code 403 Forbidden")]
    [MemberData(nameof(InvalidRoles))]
    public async Task Register_ValidationRoles_ReturnStatusCodeForbidden(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("Register");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoRegister = new UserRegisterRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "@Tester1256"
        };

        UserCommandService
            .Setup(s => s.RegisterAsync(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(true);

        var result = await UserController.Register(dtoRegister);

        var status = UserController.Response.StatusCode;

        Assert.True(result);
        Assert.Equal(statusForbidden, status);

        UserCommandService
            .Verify(s => s.RegisterAsync(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<UserCredential>()), Times.Once);
    }
}