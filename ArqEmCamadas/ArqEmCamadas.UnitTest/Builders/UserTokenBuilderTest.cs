using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class UserTokenBuilderTest
{
    private Guid _userId = Guid.NewGuid();
    private string _loginProvider = "TestProvider";
    private string _name = "TestToken";
    private string? _value = "TestTokenValue";

    public static UserTokenBuilderTest NewObject() => new();

    public UserToken Build() =>
        new()
        {
            UserId = _userId,
            LoginProvider = _loginProvider,
            Name = _name,
            Value = _value
        };

    public UserTokenBuilderTest WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public UserTokenBuilderTest WithLoginProvider(string loginProvider)
    {
        _loginProvider = loginProvider;
        return this;
    }

    public UserTokenBuilderTest WithName(string name)
    {
        _name = name;
        return this;
    }

    public UserTokenBuilderTest WithValue(string? value)
    {
        _value = value;
        return this;
    }
}
