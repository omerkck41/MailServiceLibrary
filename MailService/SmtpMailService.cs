using System.Net.Mail;

namespace EmailService
{
	/// <summary>
	/// var smtpClients = new List<SmtpClientWithStatistics>
	/// {
	///	 new SmtpClientWithStatistics
	///	 {
	///		Host = "smtp-mail.outlook.com",
	///		Port = 587,
	///		EnableSsl = true,
	///		DeliveryMethod = SmtpDeliveryMethod.Network,
	///		UseDefaultCredentials = false,
	///		Credentials = new NetworkCredential("microsoft@hotmail.com", "******", "Microsoft")
	///	 },
	///	 new SmtpClientWithStatistics
	///	 {
	///		Host = "smtp-mail.outlook.com",
	///		Port = 587,
	///		EnableSsl = true,
	///		DeliveryMethod = SmtpDeliveryMethod.Network,
	///		UseDefaultCredentials = false,
	///		Credentials = new NetworkCredential("microsoft@hotmail.com", "******", "Microsoft")
	///	 }
	/// };
	///
	/// var smtpClientSelector = new RateLimitingSmtpClientSelector(smtpClients);
	/// var mailService = new SmtpMailService(smtpClientSelector);
	///
	/// var emailMessage = new EmailMessageBuilder()
	///
	///	.AddTo("to@example.com", "Recipient Name")
	///	.AddCc("cc@example.com")
	///	.AddBcc("bcc@example.com")
	///	.AddSubject("Hello, World!")
	///	.AddContent("<h1>Hello, World!</h1>", isHtml: true)
	///	.MarkAsImportant()
	///	.Build();
	///
	/// await mailService.SendEmailAsync(emailMessage);
	/// </summary>
	public class SmtpMailService : IMailService
	{
		private readonly ISmtpClientSelector _smtpClientSelector;

		/// <summary>
		/// Initializes a new instance of the SmtpMailService class.
		/// </summary>
		/// <param name="smtpClientSelector">The SMTP client selector.</param>
		public SmtpMailService(ISmtpClientSelector smtpClientSelector)
		{
			_smtpClientSelector = smtpClientSelector;
		}

		/// <summary>
		/// Sends an e-mail message asynchronously.
		/// </summary>
		/// <param name="emailMessage">The e-mail message to send.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">Thrown when emailMessage is null.</exception>
		public async Task SendEmailAsync(EmailMessage emailMessage)
		{
			if (emailMessage == null)
				throw new ArgumentNullException(nameof(emailMessage), "Email message cannot be null.");

			// Get the client from the selector
			var smtpClient = _smtpClientSelector.GetNextClient() ?? throw new InvalidOperationException("No available SMTP client.");

			var credentials = smtpClient.Credentials!.GetCredential(smtpClient.Host!, smtpClient.Port, "Basic");

			var mailMessage = new MailMessage
			{
				From = new MailAddress(credentials?.UserName ?? emailMessage.FromAddress.Address, credentials?.Domain),
				Subject = emailMessage.Subject,
				Body = emailMessage.Content,
				IsBodyHtml = emailMessage.IsBodyHtml,
				Priority = emailMessage.IsImportant ? MailPriority.High : MailPriority.Normal
			};

			emailMessage.ToAddresses.ForEach(a => mailMessage.To.Add(new MailAddress(a.Address, a.Name)));
			emailMessage.CcAddresses.ForEach(a => mailMessage.CC.Add(a));
			emailMessage.BccAddresses.ForEach(a => mailMessage.Bcc.Add(a));
			emailMessage.Attachments.ForEach(a => mailMessage.Attachments.Add(a));

			
			try
			{
				await smtpClient.SendMailAsync(mailMessage);
				_smtpClientSelector.RegisterSend(smtpClient);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(ex.ToString());
			}
		}
	}
}