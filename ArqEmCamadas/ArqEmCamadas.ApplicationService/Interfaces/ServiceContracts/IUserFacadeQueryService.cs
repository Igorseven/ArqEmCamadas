using System.Linq.Expressions;
using System.Security.Claims;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IUserFacadeQueryService
{
    Task<bool> CheckIfTheUserExistsAsync(Expression<Func<User, bool>> predicate);
    Task<List<Claim>> FindAllUserRolesAndClaimsAsync(string userName, Guid systemOrigin);
    Task<string?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest);
    Task<User?> FindUserByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool toQuery = false);
}