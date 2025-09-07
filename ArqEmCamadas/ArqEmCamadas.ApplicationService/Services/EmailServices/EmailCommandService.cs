using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ArqEmCamadas.ApplicationService.Dtos.EmailDtos.Request;
using ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;
using ArqEmCamadas.ApplicationService.Services.EmailServices.Templates;
using ArqEmCamadas.Domain.Constants;
using ArqEmCamadas.Domain.Enum;
using ArqEmCamadas.Domain.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArqEmCamadas.ApplicationService.Services.EmailServices;

public class EmailCommandService(
    IOptions<EmailSettingsOptions> emailProvider,
    IOptions<CorsConfigurationOptions> corsProvider,
    ILogger<EmailCommandService> logger) : IEmailCommandService
{
    private readonly EmailSettingsOptions _emailProvider = emailProvider.Value;
    private readonly CorsConfigurationOptions _corsProvider = corsProvider.Value;
    private const string Format = "html";

    public async Task SendEmailAsync(EmailSenderRequest dtoRequest, ETemplateType type)
    {
        if (!_emailProvider.Active)
        {
            logger.LogInformation("Email service is disabled");
            return;
        }

        try
        {
            dtoRequest.ClientSideUrl = _corsProvider.Web;

            var (body, subject) = GetTemplate(dtoRequest, type);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailProvider.SenderName, _emailProvider.SenderEmail));
            message.To.Add(new MailboxAddress(string.Empty, dtoRequest.Email));
            message.Subject = subject;

            message.Body = new TextPart(Format)
            {
                Text = body
            };

            using var client = new SmtpClient();
            var secureSocketOptions = _emailProvider.Ssl 
                ? SecureSocketOptions.SslOnConnect 
                : SecureSocketOptions.StartTls;
            
            var port = secureSocketOptions == SecureSocketOptions.SslOnConnect
                ? _emailProvider.PortSslOrTls
                : _emailProvider.PortStartTls;
            
            if (_emailProvider is { Ssl: false })
            {
                secureSocketOptions = SecureSocketOptions.None;
            }

            await client.ConnectAsync(_emailProvider.ServerAddress, port, secureSocketOptions);
            await client.AuthenticateAsync(_emailProvider.SenderEmail, _emailProvider.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao enviar email para: {Email}. Servidor: {Server}:{Port}/{PortTls}, SSL: {Ssl}",
                dtoRequest.Email,
                _emailProvider.ServerAddress,
                _emailProvider.PortSslOrTls,
                _emailProvider.PortStartTls,
                _emailProvider.Ssl);
            
            throw;
        }
    }

    private static (string body, string subject) GetTemplate(EmailSenderRequest dtoRequest, ETemplateType type) =>
        type switch
        {
            ETemplateType.RegisterAnalyst =>
                (EmailTemplate.RegisterModel(dtoRequest), EmailSubjectConst.RegisterAnalyst),
            ETemplateType.ResetPassword =>
                (EmailTemplate.RecoveryPasswordModel(dtoRequest), EmailSubjectConst.ResetPassword),
            ETemplateType.UserRegistration =>
                (EmailTemplate.UserRegistrationModel(dtoRequest), EmailSubjectConst.UserRegistration),
            _ =>
                (string.Empty, string.Empty)
        };
}