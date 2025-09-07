using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IUserRoleRepository : IDisposable
{
    Task<bool> SaveAsync(UserRole userRole);
    Task<bool> DeleteAsync(Expression<Func<UserRole, bool>> predicate);
}