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

public sealed class RegisterAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task RegisterAsync_PerfectSetting_ReturnTrue()
    {
        var dtoRequest = new UserRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "SecurePassword123!"
        };
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        var user = UserBuilderTest.NewObject().Build();

        var role = RoleBuilderTest.NewObject().Build();

        UserRepository
            .Setup(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()))
            .ReturnsAsync(false);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(role);
        UserMapper
            .Setup(m => m.DtoRegisterToDomain(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<Guid>()))
            .Returns(user);
        UserValidator
            .Setup(v => v.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);
        UserRepository
            .Setup(r => r.SaveAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
        LoggerHandler
            .Setup(l => l.CreateLogger(It.IsAny<DomainLogger>()));

        var result = await UserCommandService.RegisterAsync(dtoRequest, credential);

        Assert.True(result);

        UserRepository
            .Verify(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoRegisterToDomain(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<Guid>()), Times.Once);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository
            .Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "invalid data")]
    public async Task RegisterAsync_InvalidData_ReturnFalse()
    {
        var dtoRequest = new UserRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "SecurePassword123!"
        };
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };

        var user = UserBuilderTest.NewObject().Build();

        var role = RoleBuilderTest.NewObject().Build();
        
        CreateNotification();

        UserRepository
            .Setup(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()))
            .ReturnsAsync(false);
        RoleFacadeQueryService
            .Setup(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()))
            .ReturnsAsync(role);
        UserMapper
            .Setup(m => m.DtoRegisterToDomain(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<Guid>()))
            .Returns(user);
        UserValidator
            .Setup(v => v.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);
        
        var result = await UserCommandService.RegisterAsync(dtoRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Once);
        UserMapper
            .Verify(m => m.DtoRegisterToDomain(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<Guid>()), Times.Once);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository
            .Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "email already exists")]
    public async Task RegisterAsync_EmailAlreadyExists_ReturnFalse()
    {
        var dtoRequest = new UserRegisterRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "SecurePassword123!"
        };
        
        var credential = new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };
        
        UserRepository
            .Setup(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()))
            .ReturnsAsync(true);

        var result = await UserCommandService.RegisterAsync(dtoRequest, credential);

        Assert.False(result);

        UserRepository
            .Verify(r => r.ExistsByPredicateAsync(
                UtilTools.BuildPredicateFunc<User>()), Times.Once);
        RoleFacadeQueryService
            .Verify(r => r.FindRoleByPredicateAsync(
                UtilTools.BuildPredicateFunc<Role>(),
                UtilTools.BuildSelectorFunc<Role>(),
                It.IsAny<bool>()), Times.Never);    
        UserMapper
            .Verify(m => m.DtoRegisterToDomain(
                It.IsAny<UserRegisterRequest>(),
                It.IsAny<Guid>()), Times.Never);
        UserValidator
            .Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Never);
        UserRepository
            .Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler
            .Verify(l => l.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}