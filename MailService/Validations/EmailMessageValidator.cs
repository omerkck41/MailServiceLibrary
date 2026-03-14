using FluentValidation;
using MailService.Domain;

namespace MailService.Validations;

/// <summary>
/// EmailMessage için modern .NET 10 doğrulama kuralları.
/// </summary>
public class EmailMessageValidator : AbstractValidator<EmailMessage>
{
    public EmailMessageValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty().WithMessage("Gönderici adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir gönderici e-posta adresi giriniz.");

        RuleFor(x => x.To)
            .NotEmpty().WithMessage("En az bir alıcı olmalıdır.")
            .Must(to => to.All(email => !string.IsNullOrWhiteSpace(email)))
            .WithMessage("Alıcı listesinde geçersiz e-posta adresleri var.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("E-posta konusu boş olamaz.")
            .MaximumLength(200).WithMessage("Konu 200 karakterden uzun olamaz.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("E-posta içeriği boş olamaz.");
    }
}
