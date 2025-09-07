using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Infra.Repository;

public sealed class UserAuthenticationRepository(
    SignInManager<User> signInManager)
    : IUserAuthenticationRepository
{
    public async Task<SignInResult> UserAuthenticationAsync(string login, string password) =>
        await signInManager.PasswordSignInAsync(login, password, false, true);

    public async Task<SignInResult> UserAuthenticationWithoutPasswordAsync(string login)
    {
        var user = await signInManager.UserManager.FindByNameAsync(login);
        
        if (user == null)
            return SignInResult.Failed;

        await signInManager.SignInAsync(user, false);
        
        return SignInResult.Success;
    }

    public Task UserSignOutAsync() => signInManager.SignOutAsync();
}