using System.Net.Mail;

namespace EmailService
{
	public class EmailMessage
	{
		public (string Address, string Name) FromAddress { get; set; }
		public List<(string Address, string Name)> ToAddresses { get; set; } = new List<(string Address, string Name)>();
		public List<string> CcAddresses { get; set; } = new List<string>();
		public List<string> BccAddresses { get; set; } = new List<string>();
		public string? Subject { get; set; }
		public string? Content { get; set; }
		public bool IsBodyHtml { get; set; }
		public bool IsImportant { get; set; }
		public List<Attachment> Attachments { get; set; } = new List<Attachment>();
	}
}