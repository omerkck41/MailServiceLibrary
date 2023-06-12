using System.Net.Mail;
using System.Net;
using EmailService;

class Program
{
	static async Task Main(string[] args)
	{
		// Bu, basit bir örnektir ve gerçek bir uygulamada SMTP istemcilerinizi
		// örneğin bir yapılandırma dosyasından veya bir hizmetten almak isteyebilirsiniz.
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
		var mailService = new SmtpMailService(smtpClientSelector);

		var emailMessage = new EmailMessageBuilder()

			.AddTo("omer_kck@msn.com", "Recipient Name")
			//.AddCc("cc@example.com")
			//.AddBcc("bcc@example.com")
			.AddSubject("Hello, World!")
			.AddContent("<h1>Hello, World!</h1>", isHtml: true)
			.MarkAsImportant()
			.Build();

		await mailService.SendEmailAsync(emailMessage);

		Console.WriteLine("Email sent!");
	}
}
