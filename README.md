# MailServiceLibrary v3.0 PRO (.NET 10.0)

🚀 **MailServiceLibrary**, modern .NET 10 standartlarında geliştirilmiş, **Clean Architecture**, **DDD** ve **Onion Architecture** projeleriyle %100 uyumlu bir e-posta gönderim kütüphanesidir. Hem ücretsiz (SMTP/MailKit) hem de profesyonel (SendGrid API) sağlayıcıları tek bir arayüzden yönetmenizi sağlar.

---

## 🌟 Öne Çıkan Özellikler

- **.NET 10.0 LTS:** En güncel performans ve dil özellikleri (Primary Constructors, Records).
- **MailKit & MimeKit:** Güvenli ve asenkron (I/O) SMTP altyapısı.
- **SendGrid API:** Bulut dostu, yüksek teslimat oranlı API desteği.
- **Otomatik Validasyon:** `FluentValidation` ile mail gönderilmeden önce hatalar yakalanır.
- **Provider Pattern:** Tek bir `IMailService` arayüzü, arkada değişebilir sağlayıcılar.
- **Clean Arch Ready:** Bağımlılık yönetimi (DI) için hazır extension metodları.

---

## ⚙️ Yapılandırma (Configuration)

Modern projelerde mail bilgilerinizi `appsettings.json` dosyasında saklamanız önerilir.

### 1. appsettings.json Örneği

```json
{
  "MailSettings": {
    "Provider": "Smtp", // Veya "SendGrid"
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": 587,
      "UserName": "senin-emailin@gmail.com",
      "Password": "uygulama-sifresi"
    },
    "SendGrid": {
      "ApiKey": "SG.xxx_yyy_zzz"
    }
  }
}
```

---

## 🛠 Projeye Entegrasyon (Dependency Injection)

Kütüphaneyi her türlü mimariye (Clean Architecture, DDD vb.) dahil etmek çok kolaydır.

### Clean Architecture Kullanımı

Bu kütüphaneyi **Infrastructure** katmanında implement edip, **Application** katmanında `IMailService` arayüzünü kullanmalısınız.

**Program.cs (veya DI Registration Sınıfı):**

```csharp
using MailService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. SEÇENEK: SMTP (Ücretsiz - Gmail/Outlook vb.)
builder.Services.AddSmtpMailService(options => {
    options.Host = builder.Configuration["MailSettings:Smtp:Host"]!;
    options.Port = int.Parse(builder.Configuration["MailSettings:Smtp:Port"]!);
    options.UserName = builder.Configuration["MailSettings:Smtp:UserName"]!;
    options.Password = builder.Configuration["MailSettings:Smtp:Password"]!;
});

// 2. SEÇENEK: SendGrid (Kurumsal - API)
// builder.Services.AddSendGridMailService(builder.Configuration["MailSettings:SendGrid:ApiKey"]!);
```

---

## 📧 Uygulama İçinde Kullanım

Herhangi bir serviste veya controller'da `IMailService`'i inject ederek kullanın:

```csharp
using MailService.Abstractions;
using MailService.Domain;

public class WelcomeService(IMailService mailService) // Modern .NET 10 Primary Constructor
{
    public async Task SendWelcomeEmailAsync(string customerEmail)
    {
        var email = new EmailMessage
        {
            From = "noreply@sirketim.com",
            To = [customerEmail], // .NET 10 Collection Expression
            Subject = "Hoş Geldiniz!",
            Body = "<h1>Merhaba!</h1><p>Sistemimize başarıyla kayıt oldunuz.</p>",
            IsHtml = true
        };

        try 
        {
            await mailService.SendEmailAsync(email);
        }
        catch (FluentValidation.ValidationException ex)
        {
            // Mail formatı hatalıysa burada yakalanır
            Console.WriteLine($"Validasyon Hatası: {ex.Message}");
        }
    }
}
```

---

## 🧪 Neden Bu Kütüphane?

1. **System.Net.Mail Yerine MailKit:** Microsoft artık `System.Net.Mail`'i önermiyor. MailKit daha güvenli, daha hızlı ve modern TLS protokollerini destekliyor.
2. **Kusursuz Soyutlama:** Yarın SMTP'den SendGrid'e (veya tam tersi) geçmek isterseniz, iş mantığınızdaki (Business Logic) kodun tek bir satırını bile değiştirmenize gerek kalmaz.
3. **Validasyon Güvencesi:** Boş konu başlığı veya geçersiz mail formatı gibi hatalar, mail sağlayıcısına gönderilmeden önce engellenir, maliyet ve zaman tasarrufu sağlar.

---
*GEMINI.md v9.0 PRO Standartlarına göre modernize edilmiştir.*
