using EmailService;
using NUnit.Framework;
using System.Net;
using System.Net.Mail;

internal class Program
{
	private async void Main(string[] args)
	{
		await SendEmailAsync_ValidEmail_DoesNotThrowException();
	}

	private async Task SendEmailAsync_ValidEmail_DoesNotThrowException()
	{
		var smtpClients = new List<SmtpClientWithStatistics>
		{
			new SmtpClientWithStatistics
			{
				TargetName = "KUKA Solutions Dep.",
				Host = "smtp-mail.outlook.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("ben_omer_sen@hotmail.com", "554654amk.","KUKA Solutions Dep.")
			},
			new SmtpClientWithStatistics
			{
				Host = "cmail10.webkontrol.doruk.net.tr",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("okucuk@gemont.net", "554654aq.","KUKA Solutions Dep.")
			}
		};

		var smtpClientSelector = new RateLimitingSmtpClientSelector(smtpClients);
		var service = new SmtpMailService(smtpClientSelector);

		var emailMessage = new EmailMessageBuilder()
			.AddTo("omer_kck@msn.com", "Recipient Name")
				//.AddCc("cc@example.com")
				//.AddBcc("bcc@example.com")
				.AddSubject("Hello, World!")
				.AddContent("<h1>Hello, World!</h1>", isHtml: true)
				.MarkAsImportant()
				.Build();

		Assert.DoesNotThrowAsync(async () => await service.SendEmailAsync(emailMessage));
	}
}