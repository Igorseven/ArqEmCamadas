using System.Text.RegularExpressions;

namespace ArqEmCamadas.Domain.Extensions;

public static partial class PasswordEncryptionExtension
{
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,20}$")]
    private static partial Regex PasswordWithRegex();
    
    
    public static bool ValidatePassword(this string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        var regex = PasswordWithRegex();
        var match = regex.Match(password);
        return match.Success;
    }

    public static string GeneratePassword(this string name) =>
        $"@{name[..new Random().Next(6, Math.Min(10, name.Length))]}2025Xb";

    public static string GenerateOwnerPassword(this string email) =>
        $"@{char.ToUpper(email[0])}{email[1..email.IndexOf('@')]}2025Xt";
}