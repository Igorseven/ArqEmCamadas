using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class RoleClaimBuilderTest
{
    private int _id = 1;
    private Guid _roleId = Guid.NewGuid();
    private string? _claimType = "TestClaimType";
    private string? _claimValue = "TestClaimValue";

    public static RoleClaimBuilderTest NewObject() => new();

    public RoleClaim Build() =>
        new()
        {
            Id = _id,
            RoleId = _roleId,
            ClaimType = _claimType,
            ClaimValue = _claimValue
        };

    public RoleClaimBuilderTest WithId(int id)
    {
        _id = id;
        return this;
    }

    public RoleClaimBuilderTest WithRoleId(Guid roleId)
    {
        _roleId = roleId;
        return this;
    }

    public RoleClaimBuilderTest WithClaimType(string? claimType)
    {
        _claimType = claimType;
        return this;
    }

    public RoleClaimBuilderTest WithClaimValue(string? claimValue)
    {
        _claimValue = claimValue;
        return this;
    }
}
