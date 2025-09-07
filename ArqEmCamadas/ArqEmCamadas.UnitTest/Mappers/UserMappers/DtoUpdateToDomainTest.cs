using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers;

public class DtoUpdateToDomainTest : UserMapperSetup
{
    [Fact]
    public void DtoUpdateToDomain_ReturnUpdatedUser()
    {
        var user = UserBuilderTest
            .NewObject()
            .WithName("Old Name")
            .Build();

        var dtoUpdate = new UserUpdateRequest()
        {
            Id = user.Id,
            Name = "Updated Name"
        };

        var result = UserMapper.DtoUpdateToDomain(user, dtoUpdate);

        Assert.Equal(dtoUpdate.Name, result.Name);
        Assert.Same(user, result);
    }
}
