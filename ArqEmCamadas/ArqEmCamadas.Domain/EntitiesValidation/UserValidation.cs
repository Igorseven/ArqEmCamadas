using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Handlers.ValidationSettings;
using FluentValidation;

namespace ArqEmCamadas.Domain.EntitiesValidation;

public sealed class UserValidation : Validate<User>
{
    public UserValidation()
    {
        SetRules();
    }

    private void SetRules()
    {
        RuleFor(u => u.UserName)
            .EmailAddress()
            .NotEmpty()
            .Length(3, 255)
            .WithMessage(u => !string.IsNullOrWhiteSpace(u.UserName)
                ? EMessage.MoreExpected.GetDescription().FormatTo("Email", "entre {MinLength} a {MaxLength}")
                : EMessage.Required.GetDescription().FormatTo("Email"));

        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage(EMessage.InvalidFormat.GetDescription().FormatTo("Email"))
            .When(u => !string.IsNullOrWhiteSpace(u.Email));

        RuleFor(u => u.Name)
            .NotEmpty()
            .Length(3, 150)
            .WithMessage(u => !string.IsNullOrWhiteSpace(u.Name)
                ? EMessage.MoreExpected.GetDescription().FormatTo("Name", "entre {MinLength} a {MaxLength}")
                : EMessage.Required.GetDescription().FormatTo("Name"));
        
        RuleFor(u => u.PasswordHash)
            .NotEmpty()
            .Must(p => p.ValidatePassword())
            .WithMessage(EMessage.InvalidFormat.GetDescription().FormatTo("Senha"));
    }
}