using MailService.Domain;

namespace MailService.Abstractions;

/// <summary>
/// Tüm mail servisleri (Smtp, SendGrid vb.) bu arayüzü uygular.
/// </summary>
public interface IMailProvider
{
    string Name { get; }
    Task<bool> SendAsync(EmailMessage message);
}

/// <summary>
/// Ana servis arayüzü.
/// </summary>
public interface IMailService
{
    Task SendEmailAsync(EmailMessage message);
}
