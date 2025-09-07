namespace ArqEmCamadas.Domain.Extensions;

public static class DateTimeExtension
{
    public static DateTime GetDateAndTimeInBrasilia(this DateTime date) =>
        date.ToUniversalTime().AddHours(-3);
    
    public static string ToBrazilianFormat(this DateTime date) =>
        date.ToString("dd/MM/yyyy HH:mm:ss");
}