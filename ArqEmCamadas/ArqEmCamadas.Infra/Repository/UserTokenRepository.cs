using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using ArqEmCamadas.Infra.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ArqEmCamadas.Infra.Repository;

public sealed class UserTokenRepository(
    ApplicationContext dbContext)
    : RepositoryBase<UserToken>(dbContext), IUserTokenRepository
{
    private const int NumberChangesInDatabase = 1;

    public Task<bool> ExistsByPredicateAsync(Expression<Func<UserToken, bool>> predicate) =>
        DbSetContext.AnyAsync(predicate);

    public async Task<bool> SaveAsync(UserToken userToken)
    {
        await DbSetContext.AddAsync(userToken);
        
        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(Expression<Func<UserToken, bool>> predicate) =>
        await DbSetContext.Where(predicate).ExecuteDeleteAsync() > NumberChangesInDatabase;
}