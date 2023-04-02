using DAL.Entity;

namespace BLL.Service.Mails
{
	public interface IMailService
	{
		Task CreateAndSendConfirmationCode(User user);

		Task<bool> MakeSubscription(int gameId, string userEmail);
	}
}