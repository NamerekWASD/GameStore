using BLL.DTO.Payment;
using Braintree;

namespace BLL.Service.BrainTree
{
	public class BraintreeService : IBraintreeService
	{
		private readonly BraintreeSettings _config;

		public BraintreeService(BraintreeSettings config)
		{
			_config = config;
		}

		public IBraintreeGateway CreateGateway()
		{
			var newGateway = new BraintreeGateway()
			{
				Environment = Braintree.Environment.SANDBOX,
				MerchantId = _config.MerchantId,
				PublicKey = _config.PublicKey,
				PrivateKey = _config.PrivateKey
			};

			return newGateway;
		}

		public IBraintreeGateway GetGateway()
		{
			return CreateGateway();
		}
	}
}