using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers;

public class DomainToSimpleDDtoResponseTest : UserMapperSetup
{
    [Fact]
    public void DomainToSimpleResponse_ReturnUserAnalystResponse()
    {
        var user = UserBuilderTest
            .NewObject()
            .Build();

        var result = UserMapper.DomainToSimpleDtoResponse(user);
        
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.UserName, result.Email);
        Assert.Equal(user.PhoneNumber, result.Phone);
    }
}
