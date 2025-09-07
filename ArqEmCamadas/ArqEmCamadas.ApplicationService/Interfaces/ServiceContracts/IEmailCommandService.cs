using ArqEmCamadas.ApplicationService.Dtos.EmailDtos.Request;
using ArqEmCamadas.Domain.Enum;

namespace ArqEmCamadas.ApplicationService.Interfaces.ServiceContracts;

public interface IEmailCommandService
{
    Task SendEmailAsync(EmailSenderRequest dtoRequest, ETemplateType type);
}