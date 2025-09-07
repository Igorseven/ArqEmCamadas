using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests.Base;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ArqEmCamadas.UnitTest.Services.UserServices.UserCommandServiceTests;

public sealed class RegisterFacadeAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task RegisterFacadeASync_PerfectSetting_ReturnTrue()
    {
        var dtoFacadeRegister = new UserFacadeRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Document = "12345678909",
            Password = "SecurePassword123!",
            Owner = Guid.NewGuid()
        };

        var user = UserBuilderTest.NewObject().Build();

        UserMapper
            .Setup(m => m.DtoFacadeRegisterToDomain(
                It.IsAny<UserFacadeRegisterRequest>()))
            .Returns(user);
        UserRepository
            .Setup(r => r.SaveAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await UserCommandService.RegisterFacadeASync(dtoFacadeRegister);

        Assert.True(result);

        UserMapper
            .Verify(m => m.DtoFacadeRegisterToDomain(
                It.IsAny<UserFacadeRegisterRequest>()), Times.Once);
        UserRepository
            .Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Repository failure")]
    public async Task RegisterFacadeASync_RepositoryFailure_ReturnFalse()
    {
        var dtoFacadeRegister = new UserFacadeRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Document = "12345678909",
            Password = "SecurePassword123!",
            Owner = Guid.NewGuid()
        };

        var user = UserBuilderTest.NewObject().Build();

        var identityErrors = new List<IdentityError>
        {
            new() { Code = "DuplicateEmail", Description = "Email already exists" }
        };

        UserMapper
            .Setup(m => m.DtoFacadeRegisterToDomain(
                It.IsAny<UserFacadeRegisterRequest>()))
            .Returns(user);
        UserRepository
            .Setup(r => r.SaveAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        var result = await UserCommandService.RegisterFacadeASync(dtoFacadeRegister);

        Assert.False(result);

        UserMapper
            .Verify(m => m.DtoFacadeRegisterToDomain(It.IsAny<UserFacadeRegisterRequest>()), Times.Once);
        UserRepository
            .Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Once);
    }
}
