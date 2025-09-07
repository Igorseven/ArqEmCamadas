using ArqEmCamadas.Api.Controllers;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Domain.UserPolicies;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Controllers.UserControllerTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ArqEmCamadas.UnitTest.Controllers.UserControllerTests;

public class FindAllWithPagination : UserControllerSetup
{
    public static IEnumerable<object[]> ValidRoles()
    {
        yield return [Policy.Administrator];
        yield return [Policy.Owner];
    }

    [Theory]
    [Trait("GET", "Return Status Code 200 Ok")]
    [MemberData(nameof(ValidRoles))]
    public async Task FindAllWithPagination_ValidationRoles_ReturnStatusCodeOk(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("FindAllWithPagination");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoResponse = new PageList<UserPaginationResponse>();

        UserQueryService
            .Setup(s => s.FindAllWithPaginationAsync(
                It.IsAny<UserPageParams>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(dtoResponse);

        var result = await UserController.FindAllWithPagination(It.IsAny<UserPageParams>());

        var status = UserController.Response.StatusCode;

        Assert.Empty(result.Items);
        Assert.Equal(statusOk, status);

        UserQueryService
            .Verify(s => s.FindAllWithPaginationAsync(
                It.IsAny<UserPageParams>(),
                It.IsAny<UserCredential>()), Times.Once);
    }

    public static IEnumerable<object[]> InvalidRoles()
    {
        yield return [Policy.System];
    }

    [Theory]
    [Trait("GET", "Return Status Code 403 Forbidden")]
    [MemberData(nameof(InvalidRoles))]
    public async Task FindAllWithPagination_ValidationRoles_ReturnStatusCodeForbidden(string role)
    {
        const int statusForbidden = 403;
        const int statusOk = 200;

        var methodInfo = typeof(UserController).GetMethod("FindAllWithPagination");

        var roles = UtilTools.GetAuthorizedControllerRoles(methodInfo!);
        
        UserController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = UtilTools.GetUserRole(role),
            Response = { StatusCode = !roles.Contains(role) ? statusForbidden : statusOk }
        };

        var dtoResponse = new PageList<UserPaginationResponse>();

        UserQueryService
            .Setup(s => s.FindAllWithPaginationAsync(
                It.IsAny<UserPageParams>(),
                It.IsAny<UserCredential>()))
            .ReturnsAsync(dtoResponse);

        var result = await UserController.FindAllWithPagination(It.IsAny<UserPageParams>());

        var status = UserController.Response.StatusCode;

        Assert.Empty(result.Items);
        Assert.Equal(statusForbidden, status);

        UserQueryService
            .Verify(s => s.FindAllWithPaginationAsync(
                It.IsAny<UserPageParams>(),
                It.IsAny<UserCredential>()), Times.Once);
    }
}
