using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;

namespace ArqEmCamadas.ApplicationService.Services.UserServices;

public sealed class UserFacadeQueryService(
    IUserRepository userRepository)
    : IUserFacadeQueryService
{
    public Task<bool> CheckIfTheUserExistsAsync(Expression<Func<User, bool>> predicate) =>
        userRepository.ExistsByPredicateAsync(predicate);

    public Task<User?> FindUserByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool toQuery = false) =>
        userRepository.FindByPredicateAsync(predicate, include, toQuery);

    public async Task<string?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(
            extractUserRequest.AccessToken,
            extractUserRequest.TokenValidationParameters,
            out var securityToken);

        if (!await ClaimNameHasIntegrityAndItsCredentialsAreActiveAsync(
                principal?.Identity?.Name,
                extractUserRequest.UserId))
        {
            return null;
        }

        return IsValidAlgorithmToken(securityToken, extractUserRequest.SecurityAlgorithm)
            ? principal!.Identity!.Name
            : null;
    }

    public async Task<List<Claim>> FindAllUserRolesAndClaimsAsync(string userName, Guid systemOrigin)
    {
        const bool active = true;

        var login = userName.ToUpper();

        var user = await userRepository.FindByPredicateAsync(
            u => u.NormalizedUserName == login,
            i => i.Include(u => u.UserRoles!
                    .Where(r => r.Role!.Active == active))
                .ThenInclude(ur => ur.Role)!,
            true);

        var claims = DefaultClaims(user!);

        if (user!.UserRoles!.Count == 0) return claims;

        var userRoles = ExtractRoles(user.UserRoles!);

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role ?? string.Empty)));

        return claims;
    }

    private static ImmutableList<string?> ExtractRoles(IEnumerable<UserRole> userRoles) =>
        userRoles.Select(roleName => roleName.Role!.Name).ToImmutableList();

    private static List<Claim> DefaultClaims(User user) =>
    [
        new(ClaimTypes.Name, user.Name),
        new(ClaimTypes.Email, user.UserName!),
        new(ClaimTypes.NameIdentifier, user.Id.ToString())
    ];

    private static bool IsValidAlgorithmToken(SecurityToken token, string securityAlgorithm) =>
        token is JwtSecurityToken jwtSecurityToken &&
        jwtSecurityToken.Header.Alg.Equals(securityAlgorithm, StringComparison.InvariantCultureIgnoreCase);

    private async Task<bool> ClaimNameHasIntegrityAndItsCredentialsAreActiveAsync(string? name, Guid userId) =>
        name is not null &&
        await userRepository.ExistsByPredicateAsync(u => u.Id == userId &&
                                                         u.UserName == name &&
                                                         u.Status == EUserStatus.Active);
}