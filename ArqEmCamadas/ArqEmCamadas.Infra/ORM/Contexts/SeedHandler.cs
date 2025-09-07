using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Seeds;

namespace ArqEmCamadas.Infra.ORM.Contexts;

public sealed class SeedHandler(
    ApplicationContext context,
    IUserRepository userRepository)
{
    private void SaveInDb() => context.SaveChanges();
    
    public async Task Seed()
    {
        var hasRole = context.Set<Role>().Any();

        if (!hasRole)
        {
            await CreateSeedForRolesAndUsers();
        }       
    }
    
    
    private async Task CreateSeedForRolesAndUsers()
    {
        var roleDbSet = context.Set<Role>();
        var roleSeeds = RoleSeed.CreateRoles();
        
        roleDbSet.AddRange(roleSeeds);
        SaveInDb();

        foreach (var user in UserSeed.CreateUserSeed())
        {
            await userRepository.SaveAsync(user);
        }

        SaveInDb();
    }
}