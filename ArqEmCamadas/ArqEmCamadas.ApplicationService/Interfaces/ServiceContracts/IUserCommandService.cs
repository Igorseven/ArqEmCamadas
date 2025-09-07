using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;
using ArqEmCamadas.Domain.ValueObjects;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IUserCommandService
{
    Task<bool> RegisterAsync(UserRegisterRequest dtoRegister,  UserCredential userCredential);
    Task<bool> UpdateAsync(UserUpdateRequest dtoUpdate, UserCredential userCredential);
    Task<bool> ActivateOrDeactivateAsync(Guid userId, UserCredential userCredential);
    Task<bool> ChangePasswordAsync(UserChangePasswordRequest dtoChangePassword, UserCredential userCredential);
    Task<bool> DeleteAsync(Guid analystId, UserCredential userCredential);
}