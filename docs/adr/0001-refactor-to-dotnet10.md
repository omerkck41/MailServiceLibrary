# ADR-0001: Modern .NET 10 ve Provider Pattern Geçişi

**Tarih:** 2026-03-14  
**Durum:** Kabul Edildi  

## Bağlam
Eski `MailService` kütüphanesi sadece SMTP odaklı ve .NET 7.0 kullanıyordu. Modern mimarilere (Clean, DDD) uyum sağlaması ve ücretli/ücretsiz servisleri desteklemesi gerekiyor.

## Karar
1. **Framework:** .NET 10.0 LTS sürümüne geçildi.
2. **Dil Özellikleri:** Primary Constructors, Required Members ve Record tipleri kullanılacak.
3. **Desen:** Provider Pattern uygulanarak SMTP, SendGrid, Mailchimp gibi servislerin kolayca tak-çıkar olması sağlanacak.
4. **Validasyon:** `FluentValidation` ile mail format kontrolü yapılacak.

## Sonuçlar
- Daha hafif ve performanslı bir kod tabanı.
- Farklı mail sağlayıcılarına (Gmail, Outlook, SendGrid) tam uyum.
- Bağımlılık yönetimi (DI) ile her türlü mimariye kolay entegrasyon.
