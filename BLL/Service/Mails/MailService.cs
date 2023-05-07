using BLL.DTO;
using BLL.DTO.Mails;
using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using DAL.UoW;
using Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text;
using UnitsOfWork.Interfaces;

namespace BLL.Service.Mails
{
    public class MailService : IMailService, ISubscriptionService
    {
        private readonly MailSettings _mailSettings;
        private readonly IUnitOfWork UoW;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IServer _server;
        private string _gameDetailsPath = string.Empty;
        private readonly ILogger<MailService> _logger;

        public MailService(MailSettings mailSettings,
            GameContext context,
            IWebHostEnvironment _appEnvironment,
            IServer _server,
            ILogger<MailService> logger)
        {
            _mailSettings = mailSettings;
            UoW = new UnitOfWork(context);
            this._appEnvironment = _appEnvironment;
            this._server = _server;
            _logger = logger;

            try
            {
                InitGameDetailsPath();
            }
            catch
            (Exception ex)
            {
                _logger.LogError("Exception occured: {0}. Inner Exception: {1}", ex.Message, ex.InnerException);
            }
        }

        private void InitGameDetailsPath()
        {
            var addressFeature = _server.Features.Get<IServerAddressesFeature>();
            if (addressFeature is null) return;
            string domain = addressFeature.Addresses.First();
            _gameDetailsPath = domain + Constants.GAME_DETAILS_PATH;
        }

        public async Task CreateAndSendConfirmationCode(User user)
        {
            if (user.Email is null) throw new NotFoundException("Користувача не знайдено");
            var stringCode = new StringBuilder();

            while (stringCode.Length < 5)
            {
                stringCode.Append(new Random().Next(0, 10));
            }
            user.ConfirmationCode = stringCode.ToString();
            await UoW.Users.ModifyAsync(user);
            await SendEmailAsync(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = ConfirmationCodeEmailTemplate.Subject,
                Body = ConfirmationCodeEmailTemplate.Body(stringCode.ToString()),
            });
        }

        public async Task<bool> MakeSubscription(int gameId, string userEmail)
        {
            var subs = await UoW.GameSubscriptions.GetAllAsync(item =>
                item.GameId == gameId && item.Email == userEmail);

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

        public async Task NotifyDiscount(GameDTO game)
        {
            if (game.DiscountPrice is null || game.Price is null) return;

            int percentage = (int)((game.DiscountPrice / game.Price * 100) - 100);

            foreach (var sub in game.Subscriptions)
            {
                var contentPath = _appEnvironment.WebRootPath;
                contentPath += @"\templates\Discount.html";
                var parsed = File.ReadAllText(contentPath);

                parsed = parsed.Replace("{imageHere}", game.Images.First().Path);
                parsed = parsed.Replace("{altHere}", game.Title);
                parsed = parsed.Replace("{titleHere}", game.Title);
                parsed = parsed.Replace("{discountHere}", game.DiscountPrice.ToString());
                parsed = parsed.Replace("{oldPriceHere}", game.Price.ToString());
                parsed = parsed.Replace("{currencyHere}", "$");
                parsed = parsed.Replace("{pecentageHere}", percentage.ToString());
                parsed = parsed.Replace("{sitePathHere}", _gameDetailsPath + game.Id);
                await SendEmailAsync(new MailRequest()
                {
                    ToEmail = sub.Email,
                    Subject = "GameStore.gg",
                    Body = parsed,
                });
            }
        }
    }

    public static class ConfirmationCodeEmailTemplate
    {
        public static string Subject => "Підтвердження Електронної пошти";

        public static string Body(string stringCode) =>
            $"<div>" +
            $"<h1 style=\"text-align: center;\">" +
            $"Код для підтвердження вашої електронної пошти" +
            $"</h1>" +
            $"<h3 style=\"text-align: center; font-size: 36px\">{stringCode}</h3>" +
            $"</div>";
    }
}