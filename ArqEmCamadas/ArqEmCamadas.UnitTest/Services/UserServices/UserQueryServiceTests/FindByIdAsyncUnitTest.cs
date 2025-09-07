using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserQueryServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserQueryServiceTests;

public sealed class FindByIdAsyncUnitTest : UserQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindById_PerfectSetting_ReturnUser()
    {
        var userId = Guid.NewGuid();

        var user = UserBuilderTest
            .NewObject()
            .Build();
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        var response = new UserSimpleResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email!,
            CellPhone = user.PhoneNumber,
            Phone = user.PhoneNumber,
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        UserMapper
            .Setup(m => m.DomainToSimpleDtoResponse(It.IsAny<User>()))
            .Returns(response);

        var result = await UserQueryService.FindByIdAsync(userId, credential);

        Assert.NotNull(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DomainToSimpleDtoResponse(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    [Trait("Query", "User not found")]
    public async Task FindById_UserNotFound_ReturnNull()
    {
        var userId = Guid.NewGuid();
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);

        var result = await UserQueryService.FindByIdAsync(userId, credential);

        Assert.Null(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DomainToSimpleDtoResponse(It.IsAny<User>()), Times.Never);
    }
}