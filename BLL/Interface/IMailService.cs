using BLL.DTO;
using DAL.Entity;

namespace BLL.Interface
{
	public interface IMailService
	{
		Task CreateAndSendConfirmationCode(User user);
		Task<bool> MakeSubscription(int gameId, string userEmail);
	}
}
