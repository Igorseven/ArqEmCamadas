using System.Linq.Expressions;
using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.ApplicationService.Projections;

public static class RoleProjection
{
    public static Expression<Func<Role, Role>> RoleNameProjection() =>
        r => new Role
        {
            Id = r.Id,
            Name = r.Name,
            NormalizedName = r.NormalizedName,
        };
}