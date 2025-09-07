using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class UserBuilderTest
{
    private Guid _id = Guid.NewGuid();
    private string _name = "John Doe";
    private string _email = "john.doe@example.com";
    private string _phone = "11985546222";
    private string _password = string.Empty;
    private EUserStatus _status = EUserStatus.Active;
    private List<UserRole> _userRoles = [];
    private List<UserClaim> _userClaims = [];
    private List<UserToken> _userTokens = [];

    public static UserBuilderTest NewObject() => new();

    public User Build() =>
        new()
        {
            Id = _id,
            UserName = _email,
            Name = _name,
            Email = _email,
            PasswordHash = _password,
            Status = _status,
            PhoneNumber = _phone,
            UserRoles = _userRoles,
            UserClaims = _userClaims,
            UserTokens = _userTokens
        };

    public UserBuilderTest WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilderTest WithName(string name)
    
    {
        _name = name;
        return this;
    }

    public UserBuilderTest WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilderTest WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilderTest WithStatus(EUserStatus status)
    {
        _status = status;
        return this;
    }
}