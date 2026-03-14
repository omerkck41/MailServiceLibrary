using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailService.Abstractions;
using MailService.Domain;
using Microsoft.Extensions.Logging;

namespace MailService.Providers;

/// <summary>
/// MailKit tabanlı modern SMTP sağlayıcısı.
/// </summary>
public sealed class SmtpMailProvider(
    string host,
    int port,
    string userName,
    string password,
    ILogger<SmtpMailProvider> logger) : IMailProvider
{
    public string Name => "MailKitSmtpProvider";

    public async Task<bool> SendAsync(EmailMessage message)
    {
        try
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(message.From));
            
            foreach (var to in message.To)
                mimeMessage.To.Add(MailboxAddress.Parse(to));
            
            foreach (var cc in message.Cc)
                mimeMessage.Cc.Add(MailboxAddress.Parse(cc));

            mimeMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.IsHtml ? message.Body : null,
                TextBody = message.IsHtml ? null : message.Body
            };

            foreach (var att in message.Attachments)
            {
                bodyBuilder.Attachments.Add(att.FileName, att.Content, ContentType.Parse(att.ContentType));
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // Gmail/Outlook gibi servisler için Auto (StartTLS) kullanımı önerilir
            await client.ConnectAsync(host, port, SecureSocketOptions.Auto);
            
            await client.AuthenticateAsync(userName, password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);

            logger.LogInformation("Email sent successfully via {Provider} (MailKit)", Name);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email via {Provider} (MailKit)", Name);
            return false;
        }
    }
}
