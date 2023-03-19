using BLL.DTO.Mails;
using BLL.Interface;
using DAL.Context;
using DAL.Entity;
using DAL.Entity.Mails;
using DAL.UoW;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		private readonly IUnitOfWork UoW;
		public MailService(MailSettings mailSettings, GameContext context)
		{
			_mailSettings = mailSettings;
			UoW = new UnitOfWork(context);
		}
		public async Task CreateAndSendConfirmationCode(User user)
		{
			var stringCode = new StringBuilder();

			while (stringCode.Length < 5)
			{
				stringCode.Append(new Random().Next(0, 10));
			}
			user.ConfirmationCode = stringCode.ToString();
			await UoW.Users.ModifyAsync(user.Id, user);
			await SendEmailAsync(new MailRequest()
			{
				ToEmail = user.Email,
				Subject = "Підтвердження Електронної пошти",
				Body = $"<div>" +
				$"<h1 style='align-text:center;'>" +
				$"Код для підтвердження вашої електронної пошти" +
				$"</h1>" +
				$"<h3 style='align-text:center;'>{stringCode}</h3>" +
				$"</div>",
			});
		}
		public async Task<bool> MakeSubscription(int gameId, string userEmail)
		{
			var subs = UoW.GameSubscriptions.GetAll(item =>
				item.GameId == gameId && item.Email == userEmail).ToBlockingEnumerable();

			if (subs.Any())
			{
				return false;
			}

			await UoW.GameSubscriptions.AddAsync(new GameSubscription
			{
				GameId = gameId,
				Email = userEmail,
			});

			return true;
		}

		private async Task SendEmailAsync(MailRequest mailRequest)
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
	}
}
