using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IUserRepository : IDisposable
{
    Task<IdentityResult> SaveAsync(User user);
    Task<IdentityResult> UpdateAsync(User user);
    Task<bool> ActivateOrDeactivateAsync(Guid userId, EUserStatus status);
    Task<IdentityResult> ChangePasswordAsync(User entity, string currentPassword, string newPassword);
    Task<IdentityResult> PasswordRecoveryAsync(User user, string newPassword);
    Task<IdentityResult> DeleteAsync(User accountIdentity);

    Task<PageList<User>> FindAllWithPaginationAsync(
        UserPageParams pageParams,
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null);

    Task<User?> FindByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool toQuery = false);

    Task<List<User>> FindAllByPredicateAsync(
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null);
    
    Task<bool> ExistsByPredicateAsync(Expression<Func<User, bool>> predicate);
}