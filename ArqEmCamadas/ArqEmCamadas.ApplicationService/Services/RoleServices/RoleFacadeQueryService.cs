using System.Linq.Expressions;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

namespace ArqEmCamadas.ApplicationService.Services.RoleServices;

public sealed class RoleFacadeQueryService(
    IRoleRepository roleRepository) : IRoleFacadeQueryService
{
    public Task<Role?> FindRoleByPredicateAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>>? projection = null,
        bool toQuery = false) =>
        roleRepository.FindByPredicateAsync(predicate, projection, toQuery);
}
