using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;
using ArqEmCamadas.ApplicationService.Interfaces.MapperContracts;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;

namespace ArqEmCamadas.ApplicationService.Mappers;

public sealed class UserMapper : IUserMapper
{
    public User DtoFacadeRegisterToDomain(UserFacadeRegisterRequest dtoFacadeRegister) =>
        new()
        {
            UserName = dtoFacadeRegister.Email,
            PasswordHash = dtoFacadeRegister.Email.GenerateOwnerPassword(),
            Name = dtoFacadeRegister.Name,
            UserRoles = [
                new UserRole
                {
                    RoleId = dtoFacadeRegister.Owner
                }
            ]
        };

    public User DtoRegisterToDomain(UserRegisterRequest dtoRegister, Guid roleId) =>
        new()
        {
            Name = dtoRegister.Name,
            UserName = dtoRegister.Email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Status = EUserStatus.Active,
            PasswordHash = dtoRegister.Email.GeneratePassword(),
            UserRoles =
            [
                new UserRole()
                {
                    RoleId = roleId
                }
            ],
        };

    public User DtoUpdateToDomain(User user, UserUpdateRequest dtoUpdate)
    {   
        user.Name = dtoUpdate.Name;

        return user;
    }

    public UserSimpleResponse DomainToSimpleDtoResponse(User domain) =>
        new ()
        {
            Name = domain.Name,
            CellPhone = domain.PhoneNumber ?? "",
            Phone = domain.PhoneNumber ?? "",
            Email = domain.UserName ?? "",
        };
    
    public PageList<UserPaginationResponse> DomainToPaginationResponse(PageList<User> users)
    {
        var userResponses = users.Items.Select(DomainToSinglePaginationResponse).ToList();
        
        return new PageList<UserPaginationResponse>(
            userResponses, 
            users.TotalCount, 
            users.CurrentPage, 
            users.PageSize);
    }

    private static UserPaginationResponse DomainToSinglePaginationResponse(User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.UserName!,
            RegistrationDate = user.RegistrationDate,
            Status = user.Status == EUserStatus.Active
        };
}