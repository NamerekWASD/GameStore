using BLL.DTO.Games;

namespace BLL.Interface
{
    public interface ISubscriptionService
    {
        Task NotifyDiscount(GameDTO game);
    }
}