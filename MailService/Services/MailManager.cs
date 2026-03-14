using FluentValidation;
using MailService.Abstractions;
using MailService.Domain;
using Microsoft.Extensions.Logging;

namespace MailService.Services;

/// <summary>
/// .NET 10 modern implementasyonu.
/// </summary>
public sealed class MailManager(
    IEnumerable<IMailProvider> providers,
    IValidator<EmailMessage> validator,
    ILogger<MailManager> logger) : IMailService
{
    public async Task SendEmailAsync(EmailMessage message)
    {
        // Önce validasyon
        var validationResult = await validator.ValidateAsync(message);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            logger.LogWarning("Mail validasyonu başarısız: {Errors}", errors);
            throw new ValidationException(errors);
        }

        // En az bir sağlayıcı olmalı
        var provider = providers.FirstOrDefault() 
            ?? throw new InvalidOperationException("Hiçbir mail sağlayıcısı yapılandırılmadı.");

        logger.LogInformation("{Provider} kullanılarak mail gönderiliyor...", provider.Name);
        await provider.SendAsync(message);
    }
}
