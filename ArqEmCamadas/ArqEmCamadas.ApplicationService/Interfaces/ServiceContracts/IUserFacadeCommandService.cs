using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IUserFacadeCommandService
{
    Task<bool> RegisterFacadeASync(UserFacadeRegisterRequest dtoFacadeRegister);
}