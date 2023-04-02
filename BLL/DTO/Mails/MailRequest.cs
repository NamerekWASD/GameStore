using Microsoft.AspNetCore.Http;

namespace BLL.DTO.Mails
{
	public class MailRequest
	{
		public string ToEmail { get; set; } = string.Empty;
		public string Subject { get; set; } = string.Empty;
		public string Body { get; set; } = string.Empty;
		public List<IFormFile>? Attachments { get; set; }
	}
}