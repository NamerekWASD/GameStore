using BLL.DTO;

namespace BLL.Service.Mails
{
	public interface ISubscriptionService
	{
		Task NotifyDiscount(GameDTO game);
	}
}