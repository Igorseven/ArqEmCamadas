using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using ArqEmCamadas.Infra.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.Infra.Repository;

public sealed class UserRoleRepository(
    ApplicationContext dbContext) 
    : RepositoryBase<UserRole>(dbContext), IUserRoleRepository
{
    private const int NumberChangesInDatabase = 1;

    public async Task<bool> DeleteAsync(Expression<Func<UserRole, bool>> predicate) =>
        await DbSetContext.Where(predicate).ExecuteDeleteAsync() == NumberChangesInDatabase;
    
    
    public async Task<bool> SaveAsync(UserRole userRole)
    {
        await DbSetContext.AddAsync(userRole);

        return await SaveInDatabaseAsync();
    }
}