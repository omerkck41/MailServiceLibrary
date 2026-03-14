# ADR-0002: System.Net.Mail Yerine MailKit Kullanımı

**Tarih:** 2026-03-14  
**Durum:** Kabul Edildi  

## Bağlam
`System.Net.Mail.SmtpClient` sınıfı Microsoft tarafından yeni geliştirmeler için önerilmemekte ve modern protokolleri (OAuth2 vb.) tam desteklememektedir. Performans ve güvenilirlik açısından MailKit sektörel standarttır.

## Karar
1. **Kütüphane:** E-posta gönderimi ve MIME mesaj oluşturma için `MailKit` ve `MimeKit` kullanılacak.
2. **Asenkron Yapı:** MailKit'in sunduğu gerçek asenkron (I/O) metodları kullanılacak.
3. **Güvenlik:** TLS/SSL yapılandırmaları ve modern kimlik doğrulama yöntemleri için altyapı hazırlanacak.

## Sonuçlar
- RFC standartlarına tam uyumlu mesaj oluşturma.
- Daha iyi performans ve asenkron işlem yönetimi.
- Modern e-posta servisleriyle (Gmail, Outlook) tam uyumluluk.
