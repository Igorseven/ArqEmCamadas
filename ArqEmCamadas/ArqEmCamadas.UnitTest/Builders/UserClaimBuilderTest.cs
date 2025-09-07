using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class UserClaimBuilderTest
{
    private int _id = 1;
    private Guid _userId = Guid.NewGuid();
    private string? _claimType = "TestClaimType";
    private string? _claimValue = "TestClaimValue";

    public static UserClaimBuilderTest NewObject() => new();

    public UserClaim Build() =>
        new()
        {
            Id = _id,
            UserId = _userId,
            ClaimType = _claimType,
            ClaimValue = _claimValue
        };

    public UserClaimBuilderTest WithId(int id)
    {
        _id = id;
        return this;
    }

    public UserClaimBuilderTest WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public UserClaimBuilderTest WithClaimType(string? claimType)
    {
        _claimType = claimType;
        return this;
    }

    public UserClaimBuilderTest WithClaimValue(string? claimValue)
    {
        _claimValue = claimValue;
        return this;
    }
}
