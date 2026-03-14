namespace MailService.Domain;

/// <summary>
/// E-posta mesajı için veri modeli.
/// .NET 10 'required' ve 'record' özellikleri kullanılmıştır.
/// </summary>
public record EmailMessage
{
    public required string From { get; init; }
    public required List<string> To { get; init; } = [];
    public List<string> Cc { get; init; } = [];
    public List<string> Bcc { get; init; } = [];
    public required string Subject { get; init; }
    public string? Body { get; init; }
    public bool IsHtml { get; init; } = true;
    public List<EmailAttachment> Attachments { get; init; } = [];
}

public record EmailAttachment(string FileName, byte[] Content, string ContentType);
