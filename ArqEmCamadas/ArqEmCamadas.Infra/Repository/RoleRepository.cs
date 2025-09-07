using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using ArqEmCamadas.Infra.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ArqEmCamadas.Infra.Repository;

public sealed class RoleRepository(
    ApplicationContext dbContext,
    RoleManager<Role> roleManager) : RepositoryBase<Role>(dbContext), IRoleRepository
{
    private const int NumberChangesInDatabase = 1;

    public Task<bool> ExistsInTheDatabaseAsync(Expression<Func<Role, bool>> predicate) =>
        DbSetContext.AnyAsync(predicate);

    public Task<Role?> FindByPredicateAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>>? projection = null,
        bool toQuery = false)
    {
        IQueryable<Role> query = DbSetContext;

        if (toQuery)
            query = query.AsNoTracking();

        if (projection is not null)
            query = query.Select(projection);

        return query.FirstOrDefaultAsync(predicate);
    }

    public Task<List<Role>> FindAllByPredicateAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>>? projection)
    {
        IQueryable<Role> query = DbSetContext;

        if (projection is not null)
            query = query.Select(projection);

        query = query.Where(predicate);

        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<Role>> FindAllByPredicateByAsync(
        Expression<Func<Role, bool>> predicate,
        Func<IQueryable<Role>, IIncludableQueryable<Role, object>>? include = null,
        bool toQuery = false)
    {
        IQueryable<Role> query = DbSetContext;
        
        if (include is not null)
            query = include(query);

        query = query.Where(predicate);

        return query.AsNoTracking().ToListAsync();
    }

    public  async Task<IdentityResult> SaveAsync(Role role) => 
        await roleManager.CreateAsync(role);

    public async Task<bool> ActivateOrDeactivateAsync(Guid roleId, bool activeOrInactive) =>
        await DbSetContext.Where(r => r.Id == roleId).ExecuteUpdateAsync(
            setter => setter.SetProperty(r => r.Active, activeOrInactive)) == NumberChangesInDatabase;
}