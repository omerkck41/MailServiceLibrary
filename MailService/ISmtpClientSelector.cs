using System.Net.Mail;

namespace EmailService
{
	public interface ISmtpClientSelector
	{
		SmtpClient GetNextClient();
		void RegisterSend(SmtpClient smtpClient);
	}
}