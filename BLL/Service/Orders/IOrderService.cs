using BLL.DTO.Orders;

namespace BLL.Service.Orders
{
	public interface IOrderService
	{
		IAsyncEnumerable<OrderDTO> GetOrders(int userId);

        Task<List<OrderDTO>> GetOrdersAsync(int userId);

		Task<OrderDTO?> GetOrder(int userId, string orderNumber);

		Task<string> CreateOrder(OrderLightDTO data);

        Task CommitChanges();
    }
}