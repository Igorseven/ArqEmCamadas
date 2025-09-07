using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;

namespace ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;

public interface IUserMapper
{
    User DtoFacadeRegisterToDomain(UserFacadeRegisterRequest dtoFacadeRegister);
    User DtoRegisterToDomain(UserRegisterRequest dtoRegister, Guid roleId);
    User DtoUpdateToDomain(User user, UserUpdateRequest dtoUpdate);
    UserSimpleResponse DomainToSimpleDtoResponse(User user);
    PageList<UserPaginationResponse> DomainToPaginationResponse(PageList<User> users);
}