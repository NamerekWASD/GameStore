using BLL.DTO.Games;

namespace BLL.Service.Mails
{
	public interface ISubscriptionService
	{
		Task NotifyDiscount(GameDTO game);
	}
}