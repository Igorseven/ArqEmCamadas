using System.Text;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Domain.Extensions;

public static class IdentityExtension
{
    public static string GetAllWritableCharacters(Encoding encoding)
    {
        encoding = Encoding.GetEncoding(encoding.WebName, new EncoderExceptionFallback(), new DecoderExceptionFallback());
        var sb = new StringBuilder();

        var chars = new char[1];
        var bytes = new byte[16];


        for (var i = 20; i <= char.MaxValue; i++)
        {
            chars[0] = (char)i;
            try
            {
                var count = encoding.GetBytes(chars, 0, 1, bytes, 0);

                if (count != 0)
                {
                    sb.Append(chars[0]);
                }
            }
            catch
            {
                break;
            }
        }
        return sb.ToString();
    }
    
    public static IEnumerable<DomainNotification> SetNotificationByIdentityResult(
        this IdentityResult identityResult, string? trace = null) =>
        identityResult.Errors.Select(error => new DomainNotification(
            trace ?? "Identity Error", 
            error.Description)).ToList();

    public static string SetNotificationBySignInResult(this SignInResult signInResult, string? trace = null)
    {
        if (signInResult.IsLockedOut) return "Atenção! Este usuário está bloqueado. Consulte o suporte";

        if (signInResult.IsNotAllowed) return "Usuário sem permissão";

        return signInResult.RequiresTwoFactor
            ? "Requer autenticação 2FA"
            : "Usuário ou senha incorretos";
    }
}