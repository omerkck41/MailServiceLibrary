# ADR-0003: SendGrid API Sağlayıcısı Ekleme

**Tarih:** 2026-03-14  
**Durum:** Kabul Edildi  

## Bağlam
Bulut ortamlarında (Azure/AWS) SMTP kısıtlamaları ve yüksek hacimli mail gönderimi ihtiyacı nedeniyle modern bir API tabanlı mail sağlayıcısı gerekmektedir.

## Karar
1. **Servis:** Sektör standardı olan `SendGrid` API desteği eklenecek.
2. **Desen:** Mevcut `IMailProvider` arayüzü korunarak `SendGridMailProvider` implemente edilecek.
3. **Konfigürasyon:** API Key tabanlı yetkilendirme kullanılacak.

## Sonuclar
- SMTP kısıtlamalarından bağımsız mail gönderimi.
- Daha yüksek teslimat (deliverability) oranları.
- Kolay genişletilebilir sağlayıcı yapısının (Provider Pattern) doğrulanması.
