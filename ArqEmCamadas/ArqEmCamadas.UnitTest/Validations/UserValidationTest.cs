using ArqEmCamadas.Domain.EntitiesValidation;
using ArqEmCamadas.UnitTest.Builders;
using ArqEmCamadas.UnitTest.TestTools;

namespace ArqEmCamadas.UnitTest.Validations;

public sealed class UserValidationTest
{
    private readonly UserValidation _validation;

    public UserValidationTest()
    {
        _validation = new UserValidation();
    }

    [Fact]
    [Trait("Validation", "Perfect setting")]
    public async Task UserValidation_PerfectSetting_ReturnTrue()
    {
        var user = UserBuilderTest.NewObject()
            .WithPassword("@Tester88569")
            .Build();

        var result = await _validation.ValidateAsync(user);

        Assert.True(result.IsValid);
    }

    public static IEnumerable<object[]> InvalidName()
    {
        yield return ["   "];
        yield return ["1"];
        yield return ["t"];
        yield return ["te"];
        yield return [" "];
        yield return [string.Empty];
        yield return [UtilTools.GenerateStringByLength(151)];
    }

    [Theory]
    [Trait("Validation", "Invalid Name")]
    [MemberData(nameof(InvalidName))]
    public async Task UserValidation_InvalidName_ReturnFalse(string name)
    {
        var user = UserBuilderTest
            .NewObject()
            .WithName(name)
            .Build();

        var result = await _validation.ValidateAsync(user);

        Assert.False(result.IsValid);
    }

    public static IEnumerable<object[]> InvalidEmail()
    {
        yield return ["   "];
        yield return ["1"];
        yield return ["t"];
        yield return ["te"];
        yield return [" "];
        yield return [string.Empty];
        yield return [UtilTools.GenerateStringByLength(256)];
    }

    [Theory]
    [Trait("Validation", "Invalid Email")]
    [MemberData(nameof(InvalidEmail))]
    public async Task UserValidation_InvalidEmail_ReturnFalse(string email)
    {
        var user = UserBuilderTest
            .NewObject()
            .WithEmail(email)
            .Build();

        var result = await _validation.ValidateAsync(user);

        Assert.False(result.IsValid);
    }
}