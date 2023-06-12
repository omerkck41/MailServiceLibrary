# MailServiceLibrary
Bu kütüphane, .NET uygulamalarında e-posta gönderme işlemlerini basitleştirmek amacıyla oluşturulmuştur. SMTP protokolü üzerinden e-postaları gönderebilmek için geniş çapta konfigürasyon seçenekleri sunar. Kütüphanenin tasarımı, birden fazla SMTP istemcisi ile çalışmayı ve dinamik olarak SMTP istemcisi seçmeyi destekler.

>Kullanıcılar, istedikleri sayıda SMTP istemcisini tanımlayabilir ve belirli bir e-postayı hangi SMTP istemcisinin göndereceğini kontrol edebilirler. Ayrıca, belirli bir zaman diliminde gönderilecek e-posta sayısını sınırlamak için bir oran sınırlama mekanizması da mevcuttur.

>Kütüphane, e-posta gönderimlerini basitleştiren ve özelleştirilebilir bir e-posta mesajı oluşturmayı sağlayan bir e-posta mesajı oluşturucusunu da içerir.

## Özellikler
* SMTP üzerinden e-posta gönderme
* Birden fazla SMTP istemcisi ile çalışma
* Dinamik SMTP istemcisi seçimi
* Oran sınırlama
* E-posta mesajı oluşturucu

## Bu projede çeşitli programlama prensipleri ve kalıpları kullanılmıştır:

- **Dependency Injection (DI):** Bu prensip, bir sınıfın bağımlılıklarını (örneğin, SmtpClient) dışarıdan almasını ve bu sayede daha esnek ve test edilebilir bir kod yazmayı sağlar. Proje boyunca DI, örneğin SmtpClientSelector sınıfının SmtpMailService sınıfına geçirilmesi ile kullanılmıştır.

* **Strategy Pattern:** Strategy kalıbı, algoritmaları bir sınıf hiyerarşisi içerisinde kapsülleyerek algoritmayı seçme yeteneğini, çalışma zamanında dinamik olarak değiştirmeye olanak sağlar. Bu projede, ISmtpClientSelector arayüzü ve onun uygulamaları olan RateLimitingSmtpClientSelector ve RoundRobinSmtpClientSelector sınıfları strategy kalıbının bir örneğidir.

* **Builder Pattern:** Builder kalıbı, karmaşık bir nesnenin oluşturulmasını daha basit ve daha anlaşılır hale getirir. Bu kalıp, EmailMessageBuilder sınıfı ile uygulanmıştır.

* **Decorator Pattern:** Decorator kalıbı, bir nesnenin işlevselliğini alt sınıflar oluşturmadan veya temel sınıf kodunu değiştirmeden dinamik olarak genişletmeye olanak sağlar. Bu proje, 'SmtpClient' nesnesinin 'SmtpClientWithStatistics' sınıfı ile genişletilmesi aracılığıyla bu kalıbı kullanmaktadır.

* **Asynchronous Programming:** .NET'in async ve await anahtar kelimeleri, ağ işlemlerini (örneğin, e-posta gönderme) gerçekleştirirken kodun okunabilirliğini ve performansını artırmak için kullanılır. Bu proje, asenkron programlama modelini geniş çapta kullanır.

## Nasıl Kullanılır?
>Örnek bir kullanım aşağıda verilmiştir. SMTP istemcilerini tanımlayın, istemci seçicisini oluşturun ve SMTP Mail Servisini oluşturun. Daha sonra, e-posta mesajınızı oluşturun ve gönderin.
```cs
public async Task SendEmailAsync_ValidEmail_DoesNotThrowException()
{
	 var smtpClients = new List<SmtpClientWithStatistics>
	 {
		 new SmtpClientWithStatistics
		 {
			Host = "smtp-mail.outlook.com",
			Port = 587,
			EnableSsl = true,
			DeliveryMethod = SmtpDeliveryMethod.Network,
			UseDefaultCredentials = false,
			Credentials = new NetworkCredential("microsoft@hotmail.com", "******", "Microsoft")
		 },
	 new SmtpClientWithStatistics
	    {
	 Host = "smtp-mail.outlook.com",
	 Port = 587,
	 EnableSsl = true,
	 DeliveryMethod = SmtpDeliveryMethod.Network,
	 UseDefaultCredentials = false,
	 Credentials = new NetworkCredential("microsoft@hotmail.com", "******", "Microsoft")
	    }
	 };
	
	 var smtpClientSelector = new RateLimitingSmtpClientSelector(smtpClients);
	 var mailService = new SmtpMailService(smtpClientSelector);
	
	 var emailMessage = new EmailMessageBuilder()
	
	   .AddTo("to@example.com", "Recipient Name")
	   .AddCc("cc@example.com")
	   .AddBcc("bcc@example.com")
	   .AddSubject("Hello, World!")
	   .AddContent("<h1>Hello, World!</h1>", isHtml: true)
	   .MarkAsImportant()
	   .Build();
	
	 await mailService.SendEmailAsync(emailMessage);
}
```
Daha fazla özelleştirme ve kullanım örneği için kodu inceleyin. Bu kütüphane, .NET projelerinde e-posta gönderme işlemlerini basitleştirmek ve SMTP istemcileri arasında kolaylıkla geçiş yapabilmek için tasarlanmıştır.
