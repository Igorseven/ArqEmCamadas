using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.Interfaces.ServiceContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ArqEmCamadas.Infra.Repository;

public class UserRepository(
    ApplicationContext dbContext,
    UserManager<User> userManager,
    IPaginationQueryService<User> paginationQueryService)
    : IUserRepository
{
    private const int NumberChangesInDatabase = 1;
    
    private DbSet<User> DbSetContext => dbContext.Set<User>();
    
    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task<bool> HaveInTheDatabaseAsync(Expression<Func<User, bool>> predicate) =>
        DbSetContext.AnyAsync(predicate);
    
    public Task<PageList<User>> FindAllWithPaginationAsync(
        UserPageParams pageParams,
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null)
    {
        IQueryable<User> query = DbSetContext;

        if (predicate is not null)
            query = query.Where(predicate);
        
        if (include is not null) 
            query = include(query);
        
        query = PaginationFilter(query, pageParams);

        query = query.OrderByDescending(u => u.RegistrationDate);

        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }

    public async Task<User?> FindByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool toQuery = false)
    {
        IQueryable<User> query = DbSetContext;

        if (toQuery)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        return await query.FirstOrDefaultAsync(predicate);
    }
    
    public async Task<List<User>> FindAllByPredicateAsync(
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null)
    {
        IQueryable<User> query = DbSetContext;

        if (predicate is not null)
            query = query.Where(predicate);
        
        if (include is not null)
            query = include(query);

        return await query.ToListAsync();
    }

    public Task<bool> ExistsByPredicateAsync(Expression<Func<User, bool>> predicate) =>
        DbSetContext.AnyAsync(predicate);

    public Task<IdentityResult> SaveAsync(User entity) =>
        userManager.CreateAsync(entity, entity.PasswordHash!);

    public Task<IdentityResult> UpdateAsync(User accountIdentity) =>
        userManager.UpdateAsync(accountIdentity);

    public async Task<bool> ActivateOrDeactivateAsync(Guid userId, EUserStatus status) =>
        await DbSetContext.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(u => u.Status, status)) == NumberChangesInDatabase;

    public async Task<IdentityResult> PasswordRecoveryAsync(User user, string newPassword)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public Task<IdentityResult> ChangePasswordAsync(User entity, string currentPassword, string newPassword)
    {
        DetachedObject(entity);

        return userManager.ChangePasswordAsync(entity, currentPassword, newPassword);
    }

    public Task<IdentityResult> DeleteAsync(User accountIdentity) =>
        userManager.DeleteAsync(accountIdentity);
    
    private void DetachedObject(User entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            DbSetContext.Attach(entity);
    }
    
    private static IQueryable<User> PaginationFilter(
        IQueryable<User> query,
        UserPageParams pageParams)
    {
        if (pageParams.Name is not null)
            query = query.Where(u => u.Name.Contains(pageParams.Name));

        if (pageParams.Email is not null)
            query = query.Where(u => u.Email!.Contains(pageParams.Email));
            
        query = query.OrderByDescending(u => u.Id);
        
        return query;
    }
}