using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests;

public sealed class DeleteAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task DeleteAsync_PerfectSetting_ReturnTrue()
    {
        var analystId = Guid.NewGuid();
        var analystRoleId = Guid.NewGuid();
        var ownerRole = RoleBuilderTest.NewObject().Build();

        var existingUser = UserBuilderTest.NewObject().Build();
        
        existingUser.UserRoles = [new() { RoleId = analystRoleId }];

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(ownerRole);
        UserRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var credential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        var result = await UserCommandService.DeleteAsync(analystId, credential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "User not found")]
    public async Task DeleteAsync_UserNotFound_ReturnFalse()
    {
        var analystId = Guid.NewGuid();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);

        var credential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        var result = await UserCommandService.DeleteAsync(analystId, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Never);
        UserRepository
            .Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Cannot delete owner")]
    public async Task DeleteAsync_OwnerUser_ReturnFalse()
    {
        var analystId = Guid.NewGuid();
        var ownerRoleId = Guid.NewGuid();
        var ownerRole = RoleBuilderTest.NewObject().WithId(ownerRoleId).Build();

        var existingUser = UserBuilderTest.NewObject().Build();
        existingUser.UserRoles = [new() { RoleId = ownerRoleId }];

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(ownerRole);

        var credential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };


        var result = await UserCommandService.DeleteAsync(analystId, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Repository failure")]
    public async Task DeleteAsync_RepositoryFailure_ReturnFalse()
    {
        var analystId = Guid.NewGuid();
        var analystRoleId = Guid.NewGuid();
        var ownerRole = RoleBuilderTest.NewObject().Build();

        var existingUser = UserBuilderTest.NewObject().Build();
        existingUser.UserRoles = [new() { RoleId = analystRoleId }];

        var identityErrors = new List<IdentityError>
        {
            new() { Code = "DeleteFailed", Description = "Delete operation failed" }
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(ownerRole);
        UserRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        var credential = new UserCredential
        {
            Id = analystId,
            Roles = []
        };

        var result = await UserCommandService.DeleteAsync(analystId, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}