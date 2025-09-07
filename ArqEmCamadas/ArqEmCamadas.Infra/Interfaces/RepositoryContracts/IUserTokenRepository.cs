using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IUserTokenRepository : IDisposable
{
    Task<bool> SaveAsync(UserToken userToken);
    Task<bool> DeleteAsync(Expression<Func<UserToken, bool>> predicate);
    Task<bool> ExistsByPredicateAsync(Expression<Func<UserToken, bool>> predicate);
}