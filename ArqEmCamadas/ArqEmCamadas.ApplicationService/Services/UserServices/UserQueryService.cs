using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Domain.ValueObjects;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.ApplicationService.Services.UserServices;

public sealed class UserQueryService(
    IUserRepository userRepository,
    IUserMapper userMapper) : IUserQueryService
{
    public async Task<UserSimpleResponse?> FindByIdAsync(Guid id, UserCredential userCredential)
    {
        var user = await userRepository.FindByPredicateAsync(
            u => u.Id == userCredential.Id,
            null,
            true);
        
        return user is not null 
        ? userMapper.DomainToSimpleDtoResponse(user)
        : null;
    }

    public async Task<PageList<UserPaginationResponse>> FindAllWithPaginationAsync(
        UserPageParams pageParams,
        UserCredential userCredential)
    {
        var usersPageList = await userRepository.FindAllWithPaginationAsync(
            pageParams,
            null,
            i => i.Include(u => u.UserRoles)!);

        return usersPageList.Items.Count > 0
            ? userMapper.DomainToPaginationResponse(usersPageList)
            : new PageList<UserPaginationResponse>();
    }



    public async Task<UserSimpleResponse?> FindByEmailAsync(string email)
    {
        var clientUser = await userRepository.FindByPredicateAsync(u => u.UserName == email);
        if (clientUser is null)
            return null;

        var clientUserClientResponse = userMapper.DomainToSimpleDtoResponse(clientUser);
        return clientUserClientResponse;
    }
}