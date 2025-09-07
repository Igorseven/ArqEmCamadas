using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers;

public class DtoRegisterToDomainTest : UserMapperSetup
{
    [Fact]
    public void DtoRegisterToDomain_ReturnUser()
    {

        var roleId = Guid.NewGuid();

        var dtoRegister = new UserRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "@john.d2025Xb"
        };
        

        var user = UserMapper.DtoRegisterToDomain(dtoRegister, roleId);

        Assert.Equal(dtoRegister.Name, user.Name);
        Assert.Equal(dtoRegister.Email, user.UserName);
        Assert.Equal(roleId, user.UserRoles?.FirstOrDefault()?.RoleId);
    }
}