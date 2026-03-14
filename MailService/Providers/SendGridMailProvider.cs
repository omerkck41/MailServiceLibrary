using SendGrid;
using SendGrid.Helpers.Mail;
using MailService.Abstractions;
using MailService.Domain;
using Microsoft.Extensions.Logging;

namespace MailService.Providers;

/// <summary>
/// SendGrid API tabanlı modern e-posta sağlayıcısı.
/// </summary>
public sealed class SendGridMailProvider(
    string apiKey,
    ILogger<SendGridMailProvider> logger) : IMailProvider
{
    public string Name => "SendGridApiProvider";

    public async Task<bool> SendAsync(EmailMessage message)
    {
        try
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(message.From);
            var subject = message.Subject;
            var toAddresses = message.To.Select(t => new EmailAddress(t)).ToList();
            
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
                from, 
                toAddresses, 
                subject, 
                message.IsHtml ? null : message.Body, 
                message.IsHtml ? message.Body : null);

            foreach (var att in message.Attachments)
            {
                await msg.AddAttachmentAsync(att.FileName, new MemoryStream(att.Content), att.ContentType);
            }

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Email sent successfully via {Provider} (API)", Name);
                return true;
            }

            var errorBody = await response.Body.ReadAsStringAsync();
            logger.LogError("Failed to send email via {Provider}. Status: {Status}, Body: {Body}", 
                Name, response.StatusCode, errorBody);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email via {Provider} (SendGrid)", Name);
            return false;
        }
    }
}
