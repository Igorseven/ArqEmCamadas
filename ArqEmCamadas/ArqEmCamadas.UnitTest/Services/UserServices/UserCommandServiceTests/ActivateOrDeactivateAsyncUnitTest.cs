using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests;

public sealed class ActivateOrDeactivateAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting - Activate user")]
    public async Task ActivateOrDeactivateAsync_InactiveUser_ReturnTrueAndActivate()
    {
        var analystId = Guid.NewGuid();
        var userCredential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };


        var existingUser = UserBuilderTest
            .NewObject()
            .WithStatus(EUserStatus.Inactive)
            .Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserRepository
            .Setup(r => r.ActivateOrDeactivateAsync(
                It.IsAny<Guid>(),
                It.IsAny<EUserStatus>()))
            .ReturnsAsync(true);
        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var result = await UserCommandService.ActivateOrDeactivateAsync(analystId, userCredential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.ActivateOrDeactivateAsync(
                analystId,
                EUserStatus.Active), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Perfect setting - Deactivate user")]
    public async Task ActivateOrDeactivateAsync_ActiveUser_ReturnTrueAndDeactivate()
    {
        var analystId = Guid.NewGuid();
        var userCredential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        var existingUser = UserBuilderTest
            .NewObject()
            .WithStatus(EUserStatus.Active)
            .Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserRepository
            .Setup(r => r.ActivateOrDeactivateAsync(
                It.IsAny<Guid>(),
                It.IsAny<EUserStatus>()))
            .ReturnsAsync(true);
        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var result = await UserCommandService.ActivateOrDeactivateAsync(analystId, userCredential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.ActivateOrDeactivateAsync(
                analystId,
                EUserStatus.Inactive), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "User not found")]
    public async Task ActivateOrDeactivateAsync_UserNotFound_ReturnFalse()
    {
        var analystId = Guid.NewGuid();
        var userCredential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);

        var result = await UserCommandService.ActivateOrDeactivateAsync(analystId, userCredential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.ActivateOrDeactivateAsync(
                It.IsAny<Guid>(),
                It.IsAny<EUserStatus>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Repository failure")]
    public async Task ActivateOrDeactivateAsync_RepositoryFailure_ReturnFalse()
    {
        var analystId = Guid.NewGuid();
        var userCredential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        var existingUser = UserBuilderTest
            .NewObject()
            .WithStatus(EUserStatus.Active)
            .Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserRepository
            .Setup(r => r.ActivateOrDeactivateAsync(
                It.IsAny<Guid>(),
                It.IsAny<EUserStatus>()))
            .ReturnsAsync(false);

        var result = await UserCommandService.ActivateOrDeactivateAsync(analystId, userCredential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.ActivateOrDeactivateAsync(
                analystId,
                EUserStatus.Inactive), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}