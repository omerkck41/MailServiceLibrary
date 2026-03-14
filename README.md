# MailServiceLibrary v3.0 PRO (.NET 10.0)

🚀 Modern, yüksek performanslı ve her projeye (Clean Architecture, DDD, Onion) saniyeler içinde dahil edilebilen e-posta gönderim kütüphanesi.

## 🌟 Yenilikler (v3.0)
- **SendGrid API Desteği:** Kurumsal ve bulut (Azure/AWS) dostu mail gönderimi.
- **MailKit & MimeKit:** Sektör standardı SMTP altyapısı.
- **Provider Pattern:** Tek bir arayüz, sınırsız sağlayıcı.

---

## 🛠 Kurulum ve Yapılandırma

### 1. Seçenek: Ücretsiz / SMTP (MailKit)
Gmail, Outlook veya özel SMTP sunucunuzu kullanmak için:

```csharp
using MailService.Extensions;

builder.Services.AddSmtpMailService(options => {
    options.Host = "smtp.gmail.com";
    options.Port = 587;
    options.UserName = "senin-emailin@gmail.com";
    options.Password = "uygulama-sifresi";
});
```

### 2. Seçenek: Kurumsal / SendGrid (API)
Yüksek hacimli ve güvenli gönderim için:

```csharp
using MailService.Extensions;

builder.Services.AddSendGridMailService("SENDGRID_API_KEY_BURAYA");
```

---

## 📧 Kullanım Örneği

Seçtiğiniz sağlayıcıdan bağımsız olarak kullanım aynıdır:

```csharp
using MailService.Abstractions;
using MailService.Domain;

public class MyService(IMailService mailService) // .NET 10 Primary Constructor
{
    public async Task SendWelcomeEmail()
    {
        var email = new EmailMessage
        {
            From = "info@sirketim.com",
            To = ["musteri@example.com"],
            Subject = "Hoş Geldiniz!",
            Body = "<h1>Merhaba!</h1><p>Sitemize başarıyla kayıt oldunuz.</p>",
            IsHtml = true
        };

        await mailService.SendEmailAsync(email);
    }
}
```

---

## 🏗 Mimari Avantajlar
- **Clean Architecture Uyumu:** `IMailService` arayüzünü Application katmanında kullanın, kütüphane detaylarını düşünmeyin.
- **Asenkron Yapı:** Tamamen I/O asenkron (Non-blocking) tasarım.
- **Dökümantasyon:** ADR (Architecture Decision Records) ile tüm mimari kararlar `docs/adr/` altında kayıtlıdır.

---
*GEMINI.md v9.0 PRO Standartlarına göre modernize edilmiştir.*
