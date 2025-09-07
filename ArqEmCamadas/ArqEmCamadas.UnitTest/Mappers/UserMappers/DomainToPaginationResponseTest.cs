using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;
using ArqEmCamadas.UnitTest.TestTools;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers;

public class DomainToPaginationResponseTest : UserMapperSetup
{
    [Fact]
    public void DomainToPaginationResponse_ReturnPageListUserPaginationResponse()
    {
        var ownerRoleId = Guid.NewGuid();
        
        var user = UserBuilderTest
            .NewObject()
            .WithStatus(EUserStatus.Active)
            .Build();

        user.UserRoles = [
            new UserRole
            {
                RoleId = ownerRoleId
            }
        ];

        var pageList = UtilTools.BuildPageList(user);

        var result = UserMapper.DomainToPaginationResponse(pageList);

        var userResponse = result.Items.FirstOrDefault()!;

        Assert.Equal(user.Id, userResponse.Id);
        Assert.Equal(user.Name, userResponse.Name);
        Assert.Equal(user.UserName, userResponse.Email);
        Assert.True(userResponse.Status);
    }
}
