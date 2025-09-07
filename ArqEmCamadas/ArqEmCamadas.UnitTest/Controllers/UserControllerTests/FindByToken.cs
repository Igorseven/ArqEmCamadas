using ArqEmCamadas.Api.Controllers;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.UserPolicies;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Controllers.UserControllerTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ArqEmCamadas.UnitTest.Controllers.UserControllerTests;

public class FindByToken : UserControllerSetup
{
    public static IEnumerable<object[]> ValidRoles()
    {
        yield return [Policy.Administrator];
        yield return [Policy.Owner];
        yield return [Policy.Analyst];
    }

    [Theory]
    [Trait("GET", "Return Status Code 200 Ok")]
    [MemberData(nameof(ValidRoles))]
    public async Task FindByToken_ValidationRoles_ReturnStatusCodeOk(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("FindByToken");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var userId = Guid.NewGuid();
        var expectedResponse = new UserSimpleResponse
        {
            Id = userId,
            Name = "John Doe",
            Email = "john.doe@example.com",
            CellPhone = "11999999999",
            Phone = "1133333333"
        };

        UserQueryService
            .Setup(s => s.FindByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(expectedResponse);

        var result = await UserController.FindById(userId);

        var status = UserController.Response.StatusCode;

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
        Assert.Equal(expectedResponse.Email, result.Email);
        Assert.Equal(statusOk, status);

        UserQueryService
            .Verify(s => s.FindByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<UserCredential>()), Times.Once);
    }

    public static IEnumerable<object[]> InvalidRoles()
    {
        yield return [Policy.System];
    }

    [Theory]
    [Trait("GET", "Return Status Code 403 Forbidden")]
    [MemberData(nameof(InvalidRoles))]
    public async Task FindByToken_ValidationRoles_ReturnStatusCodeForbidden(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("FindByToken");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var userId = Guid.NewGuid();
        var expectedResponse = new UserSimpleResponse
        {
            Id = userId,
            Name = "John Doe",
            Email = "john.doe@example.com",
            CellPhone = "11999999999",
            Phone = "1133333333"
        };

        UserQueryService
            .Setup(s => s.FindByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(expectedResponse);

        var result = await UserController.FindById(userId);

        var status = UserController.Response.StatusCode;

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
        Assert.Equal(expectedResponse.Email, result.Email);
        Assert.Equal(statusForbidden, status);

        UserQueryService
            .Verify(s => s.FindByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<UserCredential>()), Times.Once);
    }
}
