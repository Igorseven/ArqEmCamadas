using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IUserRoleRepository : IDisposable
{
    Task<bool> SaveAsync(UserRole userRole);
    Task<bool> DeleteAsync(Expression<Func<UserRole, bool>> predicate);

    Task<UserRole?> FindByPredicateAsync(
        Expression<Func<UserRole, bool>> predicate,
        Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null,
        bool toQuery = false);

    Task<List<UserRole>> FindAllByPredicateAsync(
        Expression<Func<UserRole, bool>> predicate,
        Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null);
}