using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IUserAuthenticationRepository
{
    Task<SignInResult> UserAuthenticationAsync(string login, string password);
    Task<SignInResult> UserAuthenticationWithoutPasswordAsync(string login);
    Task UserSignOutAsync();
}