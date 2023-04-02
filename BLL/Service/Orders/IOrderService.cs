using BLL.DTO.Orders;

namespace BLL.Service.Orders
{
	public interface IOrderService
	{
		IAsyncEnumerable<OrderDTO> GetOrders(int userId);

		Task<OrderDTO?> GetOrder(int userId, int orderId);

		Task<int> CreateOrder(OrderLightDTO data);

		Task CalculateTotalPrice(OrderLightDTO data);
	}
}