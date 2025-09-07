using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IRoleFacadeQueryService
{
    Task<Role?> FindRoleByPredicateAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>>? projection = null,
        bool toQuery = false);
}
