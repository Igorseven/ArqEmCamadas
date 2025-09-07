using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.UnitTest.Builders;

public sealed class UserLoginBuilderTest
{
    private string _loginProvider = "TestProvider";
    private string _providerKey = "TestProviderKey";
    private string? _providerDisplayName = "Test Provider Display Name";
    private Guid _userId = Guid.NewGuid();

    public static UserLoginBuilderTest NewObject() => new();

    public UserLogin Build() =>
        new()
        {
            LoginProvider = _loginProvider,
            ProviderKey = _providerKey,
            ProviderDisplayName = _providerDisplayName,
            UserId = _userId
        };

    public UserLoginBuilderTest WithLoginProvider(string loginProvider)
    {
        _loginProvider = loginProvider;
        return this;
    }

    public UserLoginBuilderTest WithProviderKey(string providerKey)
    {
        _providerKey = providerKey;
        return this;
    }

    public UserLoginBuilderTest WithProviderDisplayName(string? providerDisplayName)
    {
        _providerDisplayName = providerDisplayName;
        return this;
    }

    public UserLoginBuilderTest WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }
}
