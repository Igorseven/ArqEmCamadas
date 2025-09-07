using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;
using ArqEmCamadas.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests;

public sealed class ChangePasswordAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task ChangePasswordAsync_PerfectSetting_ReturnTrue()
    {
        var userId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        var changePasswordRequest = new UserChangePasswordRequest()
        {
            UserId = targetUserId,
            NewPassword = "Password123!"
        };

        var existingUser = UserBuilderTest.NewObject().Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);

        UserRepository
            .Setup(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var credential = new UserCredential
        {
            Id = userId,
            Roles = []
        };

        var result = await UserCommandService.ChangePasswordAsync(changePasswordRequest, credential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);

        UserRepository
            .Verify(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                changePasswordRequest.NewPassword), Times.Once);

        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }


    [Fact]
    [Trait("Command", "User not found")]
    public async Task ChangePasswordAsync_UserNotFound_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var changePasswordRequest = new UserChangePasswordRequest()
        {
            UserId = targetUserId,
            NewPassword = "Password123!"
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User?)null);

        var credential = new UserCredential
        {
            Id = userId,
            Roles = []
        };

        var result = await UserCommandService.ChangePasswordAsync(changePasswordRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                It.IsAny<string>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Invalid password")]
    public async Task ChangePasswordAsync_InvalidPassword_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var changePasswordRequest = new UserChangePasswordRequest()
        {
            UserId = targetUserId,
            NewPassword = "123" // Invalid password
        };

        var existingUser = UserBuilderTest.NewObject().Build();

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);

        var credential = new UserCredential
        {
            Id = userId,
            Roles = []
        };

        var result = await UserCommandService.ChangePasswordAsync(changePasswordRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                It.IsAny<string>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Repository failure")]
    public async Task ChangePasswordAsync_RepositoryFailure_ReturnFalse()
    {
        var userId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var changePasswordRequest = new UserChangePasswordRequest()
        {
            UserId = targetUserId,
            NewPassword = "Password123!"
        };

        var existingUser = UserBuilderTest.NewObject().Build();

        var identityErrors = new List<IdentityError>
        {
            new() { Code = "PasswordTooWeak", Description = "Password does not meet requirements" }
        };

        UserRepository
            .Setup(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingUser);
        UserRepository
            .Setup(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        var credential = new UserCredential
        {
            Id = userId,
            Roles = []
        };

        var result = await UserCommandService.ChangePasswordAsync(changePasswordRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.FindByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildQueryableIncludeFunc<User>(),
                It.IsAny<bool>()), Times.Once);
        UserRepository
            .Verify(r => r.PasswordRecoveryAsync(
                It.IsAny<User>(),
                changePasswordRequest.NewPassword), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}