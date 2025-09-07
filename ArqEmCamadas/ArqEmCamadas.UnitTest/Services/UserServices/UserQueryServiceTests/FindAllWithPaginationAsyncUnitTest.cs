using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserQueryServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserQueryServiceTests;

public sealed class FindAllWithPaginationAsyncUnitTest : UserQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindAllWithPagination_PerfectSetting_ReturnUsers()
    {
        var pageParams = new UserPageParams()
        {
            PageNumber = 1,
            PageSize = 10,
        };

        var user = UserBuilderTest
            .NewObject()
            .Build();

        var domainPageList = UtilTools.BuildPageList(user);

        var role = RoleBuilderTest.NewObject().Build();

        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        var dtoResponse = new UserPaginationResponse()
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Status = true,
            RegistrationDate = DateTime.Now
        };

        var dtoPageList = UtilTools.BuildPageList(dtoResponse);

        UserRepository
            .Setup(r => r.FindAllWithPaginationAsync(
                pageParams,
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>()))
            .ReturnsAsync(domainPageList);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(role);
        UserMapper
            .Setup(m => m.DomainToPaginationResponse(
                It.IsAny<PageList<User>>()))
            .Returns(dtoPageList);

        var result = await UserQueryService.FindAllWithPaginationAsync(pageParams, credential);

        Assert.NotEmpty(result.Items);

        UserRepository
            .Verify(r => r.FindAllWithPaginationAsync(
                pageParams,
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>()), Times.Once);
        UserMapper
            .Verify(m => m.DomainToPaginationResponse(It.IsAny<PageList<User>>()), Times.Once);
    }

    [Fact]
    [Trait("Query", "Empty page list")]
    public async Task FindAllWithPagination_EmptyPageList_ReturnEmptyPageList()
    {
        var pageParams = new UserPageParams()
        {
            PageNumber = 1,
            PageSize = 10,
        };
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        var domainPageList = UtilTools.BuildPageList<User>([]);

        var role = RoleBuilderTest.NewObject().Build();

        UserRepository
            .Setup(r => r.FindAllWithPaginationAsync(
                pageParams,
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>()))
            .ReturnsAsync(domainPageList);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(role);
        
        var result = await UserQueryService.FindAllWithPaginationAsync(pageParams, credential);

        Assert.Empty(result.Items);

        UserRepository
            .Verify(r => r.FindAllWithPaginationAsync(
                pageParams,
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>()), Times.Once);
        UserMapper
            .Verify(m => m.DomainToPaginationResponse(
                It.IsAny<PageList<User>>()), Times.Never);
    }
}