using BLL.DTO;
using BLL.DTO.Orders;
using Braintree;
using DAL.Entity;

namespace BLL.Service.BrainTree
{
	public interface IBraintreeService
	{
		Task<Result<Transaction>> MakeTransaction(OrderLightDTO orderlight, User user);
    }
}