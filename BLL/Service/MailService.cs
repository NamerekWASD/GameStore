using BLL.DTO;
using BLL.DTO.Cheques;
using BLL.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BLL.Service
{
    public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		public MailService(IOptions<MailSettings> mailSettings)
		{
			_mailSettings = mailSettings.Value;
		}
		public async Task SendEmailAsync(MailRequest mailRequest)
		{
			var email = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_mailSettings.Mail),
				Subject = mailRequest.Subject,
			};
			email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

			var builder = new BodyBuilder();
			if (mailRequest.Attachments != null)
			{
				byte[] fileBytes;
				foreach (var file in mailRequest.Attachments)
				{
					if (file.Length > 0)
					{
						using (var ms = new MemoryStream())
						{
							file.CopyTo(ms);
							fileBytes = ms.ToArray();
						}
						builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
					}
				}
			}
			builder.HtmlBody = mailRequest.Body;
			email.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
			smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
			await smtp.SendAsync(email);
			smtp.Disconnect(true);
		}

		public async Task SendCheque(ChequeRequest request)
		{
			string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
			StreamReader str = new StreamReader(FilePath);
			string MailText = str.ReadToEnd();
			str.Close();
			MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
			email.To.Add(MailboxAddress.Parse(request.ToEmail));
			email.Subject = $"Welcome {request.UserName}";
			var builder = new BodyBuilder();
			builder.HtmlBody = MailText;
			email.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
			smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
			await smtp.SendAsync(email);
			smtp.Disconnect(true);
		}
	}
}
