using System.Net.Mail;

namespace EmailService
{
	public class EmailMessageBuilder
	{
		private readonly EmailMessage emailMessage = new();

		public EmailMessageBuilder AddFrom(SmtpClientWithStatistics smtpClientWithStatistics)
		{
			if (smtpClientWithStatistics == null)
				throw new ArgumentNullException(nameof(SmtpClientWithStatistics), "Subject cannot be null or empty.");

			var credentials = smtpClientWithStatistics.Credentials!.GetCredential(smtpClientWithStatistics.Host!, smtpClientWithStatistics.Port, "Basic")!;
			emailMessage.FromAddress = (credentials.UserName, credentials.UserName);
			return this;
		}

		public EmailMessageBuilder AddTo(string address, string name = "")
		{
			emailMessage.ToAddresses.Add((address, name));
			return this;
		}

		public EmailMessageBuilder AddCc(string address)
		{
			emailMessage.CcAddresses.Add(address);
			return this;
		}

		public EmailMessageBuilder AddBcc(string address)
		{
			emailMessage.BccAddresses.Add(address);
			return this;
		}

		public EmailMessageBuilder AddSubject(string subject)
		{
			if (string.IsNullOrEmpty(subject))
				throw new ArgumentNullException(nameof(subject), "Subject cannot be null or empty.");

			emailMessage.Subject = subject;
			return this;
		}

		public EmailMessageBuilder AddContent(string content, bool isHtml = false)
		{
			if (string.IsNullOrEmpty(content))
				throw new ArgumentNullException(nameof(content), "Content cannot be null or empty.");

			emailMessage.Content = content;
			emailMessage.IsBodyHtml = isHtml;
			return this;
		}

		public EmailMessageBuilder MarkAsImportant()
		{
			emailMessage.IsImportant = true;
			return this;
		}

		public EmailMessageBuilder AddAttachment(Stream stream, string name)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream), "Stream cannot be null.");

			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

			emailMessage.Attachments.Add(new Attachment(stream, name));
			return this;
		}

		public EmailMessageBuilder AddAttachment(byte[] content, string name)
		{
			var stream = new MemoryStream(content);
			emailMessage.Attachments.Add(new Attachment(stream, name));
			return this;
		}

		public EmailMessage Build()
		{
			return emailMessage;
		}
	}
}