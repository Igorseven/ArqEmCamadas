using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests;

public sealed class UpdateAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task UpdateAsync_PerfectSetting_ReturnTrue()
    {
        var userId = Guid.NewGuid();
        var updateRequest = new UserUpdateRequest()
        {
            Id = Guid.NewGuid(),
            Name = "Updated John Doe"
        };

        var existingUser = UserBuilderTest.NewObject().Build();
        var updatedUser = UserBuilderTest.NewObject().Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserMapper
            .Setup(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()))
            .Returns(updatedUser);
        UserValidator
            .Setup(v => v.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);
        UserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var credential = new UserCredential { Id = userId, Roles = [] };

        var result = await UserCommandService.UpdateAsync(updateRequest, credential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()), Times.Once);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository
            .Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "User not found")]
    public async Task UpdateAsync_UserNotFound_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var updateRequest = new UserUpdateRequest()
        {
            Id = Guid.NewGuid(),
            Name = "Updated John Doe"
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);

        var credential = new UserCredential { Id = userId, Roles = [] };


        var result = await UserCommandService.UpdateAsync(updateRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()), Times.Never);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Never);
        UserRepository
            .Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Invalid data")]
    public async Task UpdateAsync_InvalidData_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var updateRequest = new UserUpdateRequest()
        {
            Id = Guid.NewGuid(),
            Name = "Updated John Doe"
        };

        var existingUser = UserBuilderTest.NewObject().Build();
        var updatedUser = UserBuilderTest.NewObject().Build();

        CreateNotification();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserMapper
            .Setup(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()))
            .Returns(updatedUser);
        UserValidator
            .Setup(v => v.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);

        var credential = new UserCredential { Id = userId, Roles = [] };

        var result = await UserCommandService.UpdateAsync(updateRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()), Times.Once);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository
            .Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Repository failure")]
    public async Task UpdateAsync_RepositoryFailure_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var updateRequest = new UserUpdateRequest()
        {
            Id = Guid.NewGuid(),
            Name = "Updated John Doe"
        };

        var existingUser = UserBuilderTest.NewObject().Build();
        var updatedUser = UserBuilderTest.NewObject().Build();

        var identityErrors = new List<IdentityError>
        {
            new() { Code = "UpdateFailed", Description = "Update operation failed" }
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserMapper
            .Setup(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()))
            .Returns(updatedUser);
        UserValidator
            .Setup(v => v.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);
        UserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        var credential = new UserCredential { Id = userId, Roles = [] };

        var result = await UserCommandService.UpdateAsync(updateRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoUpdateToDomain(
                It.IsAny<User>(),
                It.IsAny<UserUpdateRequest>()), Times.Once);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository
            .Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}