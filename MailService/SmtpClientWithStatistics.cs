using System.Net.Mail;

namespace EmailService
{
	public class SmtpClientWithStatistics : SmtpClient
	{
		public int SentEmails { get; private set; }

		public DateTime LastSent { get; private set; }

		public void RegisterSend()
		{
			SentEmails++;
			LastSent = DateTime.Now;
		}
	}
}