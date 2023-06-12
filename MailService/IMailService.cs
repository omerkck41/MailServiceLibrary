namespace EmailService
{
	public interface IMailService
	{
		Task SendEmailAsync(EmailMessage emailMessage);
	}
}