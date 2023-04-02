using Braintree;

namespace BLL.Service.BrainTree
{
	public interface IBraintreeService
	{
		IBraintreeGateway CreateGateway();

		IBraintreeGateway GetGateway();
	}
}