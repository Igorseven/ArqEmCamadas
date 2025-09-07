using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class RoleBuilderTest
{
    private Guid _id = Guid.NewGuid();
    private string? _name = "TestRole";
    private string? _normalizedName = "TESTROLE";
    private string? _concurrencyStamp = Guid.NewGuid().ToString();
    private bool _active = true;
    private List<UserRole>? _userRoles = [];
    private List<RoleClaim>? _roleClaims = [];

    public static RoleBuilderTest NewObject() => new();

    public Role Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            NormalizedName = _normalizedName,
            ConcurrencyStamp = _concurrencyStamp,
            Active = _active,
            UserRoles = _userRoles,
            RoleClaims = _roleClaims
        };

    public RoleBuilderTest WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public RoleBuilderTest WithName(string? name)
    {
        _name = name;
        return this;
    }

    public RoleBuilderTest WithNormalizedName(string? normalizedName)
    {
        _normalizedName = normalizedName;
        return this;
    }

    public RoleBuilderTest WithConcurrencyStamp(string? concurrencyStamp)
    {
        _concurrencyStamp = concurrencyStamp;
        return this;
    }

    public RoleBuilderTest WithActive(bool active)
    {
        _active = active;
        return this;
    }

    public RoleBuilderTest WithUserRoles(List<UserRole>? userRoles)
    {
        _userRoles = userRoles;
        return this;
    }

    public RoleBuilderTest WithRoleClaims(List<RoleClaim>? roleClaims)
    {
        _roleClaims = roleClaims;
        return this;
    }
}
