using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class UserRoleBuilderTest
{
    private Guid _userId = Guid.NewGuid();
    private Guid _roleId = Guid.NewGuid();
    private Role? _role;
    private User? _user;

    public static UserRoleBuilderTest NewObject() => new();

    public UserRole Build() =>
        new()
        {
            UserId = _userId,
            RoleId = _roleId,
            Role = _role,
            User = _user
        };

    public UserRoleBuilderTest WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public UserRoleBuilderTest WithRoleId(Guid roleId)
    {
        _roleId = roleId;
        return this;
    }

    public UserRoleBuilderTest WithRole(Role role)
    {
        _role = role;
        return this;
    }

    public UserRoleBuilderTest WithUser(User user)
    {
        _user = user;
        return this;
    }
}
