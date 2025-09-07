using ArqEmCamadas.ApplicationService.Mappers;

namespace ArqEmCamadas.UnitTest.Mappers.UserMappers.Base;

public abstract class UserMapperSetup
{
    protected readonly UserMapper UserMapper;

    protected UserMapperSetup()
    {
        UserMapper = new UserMapper();
    }
}