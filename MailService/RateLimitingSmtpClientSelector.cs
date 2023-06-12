using System.Net.Mail;

namespace EmailService
{
	public class RateLimitingSmtpClientSelector : ISmtpClientSelector
	{
		private readonly List<SmtpClientWithStatistics> _smtpClients;

		public RateLimitingSmtpClientSelector(List<SmtpClientWithStatistics> smtpClients)
		{
			_smtpClients = smtpClients;
		}

		public SmtpClient GetNextClient()
		{
			// Here you can implement a strategy to select the next client.
			// For instance, you might want to select the client that was used the least recently,
			// or the client that has the least amount of sent emails in the current time frame, etc.
			SmtpClientWithStatistics? clientWithLeastSends = null;

			foreach (var client in _smtpClients)
			{
				if (clientWithLeastSends == null || client.SentEmails < clientWithLeastSends.SentEmails)
				{
					clientWithLeastSends = client;
				}
			}

			return clientWithLeastSends ?? _smtpClients[0];
		}

		public void RegisterSend(SmtpClient smtpClient)
		{
			// Here you can implement a strategy to track the number of emails sent by each client.
			// You might want to keep this information to be used in the GetNextClient method.

			using var clientWithStatistics = smtpClient as SmtpClientWithStatistics ?? throw new ArgumentException("The provided SmtpClient should be of type SmtpClientWithStatistics.");

			clientWithStatistics.RegisterSend();
		}
	}
}