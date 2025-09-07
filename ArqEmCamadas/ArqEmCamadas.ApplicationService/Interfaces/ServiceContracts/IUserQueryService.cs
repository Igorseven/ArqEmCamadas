using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Domain.ValueObjects;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IUserQueryService
{
    Task<UserSimpleResponse?> FindByIdAsync(Guid id, UserCredential userCredential);
    Task<UserSimpleResponse?> FindByEmailAsync(string email);
    Task<PageList<UserPaginationResponse>> FindAllWithPaginationAsync(
        UserPageParams pageParams,
        UserCredential userCredential);
}