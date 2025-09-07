using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers;

public class DtoFacadeRegisterToDomainTest : UserMapperSetup
{
    [Fact]
    public void DtoFacadeRegisterToDomain_ReturnUser()
    {
        var dtoFacadeRegister = new UserFacadeRegisterRequest()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Document = "12345678901",
            Password = "@John.doe2025Xt",
            Owner = Guid.NewGuid()
        };
        
        var user = UserMapper.DtoFacadeRegisterToDomain(dtoFacadeRegister);

        Assert.Equal(dtoFacadeRegister.Name, user.Name);
        Assert.Equal(dtoFacadeRegister.Email, user.UserName);
        Assert.Equal(dtoFacadeRegister.Password, user.PasswordHash);
        Assert.Equal(dtoFacadeRegister.Owner, user.UserRoles?.FirstOrDefault()?.RoleId);
    }
}
