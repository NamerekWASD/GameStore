using BLL.DTO;
using BLL.DTO.Orders;
using BLL.DTO.Payment;
using BLL.Service.Games;
using BLL.Tools;
using Braintree;
using DAL.Entity;
using System.Reflection;

namespace BLL.Service.BrainTree
{
	public class BraintreeService : IBraintreeService
	{
		private readonly BraintreeSettings _config;
        private readonly IGameService _gameService;

        public BraintreeService(BraintreeSettings config,
            IGameService gameService)
		{
			_config = config;
            _gameService = gameService;
        }

		private IBraintreeGateway CreateGateway()
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

        public async Task<Result<Transaction>> MakeTransaction(OrderLightDTO orderlight, User user)
        {
            var gateway = CreateGateway();
            var games = _gameService.GetGames(orderlight.Games.Select(item => item.Id).ToArray(), CancellationToken.None).ToBlockingEnumerable();
            var transactionRequest = new TransactionRequest
            {
                OrderId = orderlight.OrderNumber.ToString(),
                Amount = orderlight.Total,
                PaymentMethodNonce = orderlight.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                },
                BillingAddress = MapperHelpers.Instance.Map<AddressRequest>(orderlight.BillingAddress),
                Customer = new CustomerRequest
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                },
                LineItems = games.Select(item => new TransactionLineItemRequest()
                {
                    Name = item.Title,
                    Quantity = orderlight.Games?.FirstOrDefault(game => game.Id == item.Id)?.Count ?? null,
                    UnitAmount = item.Price,
                    TotalAmount = item.DiscountPrice ?? item.Price,
                    LineItemKind = ResolveItemKind(orderlight.PaymentType),
                }).ToArray(),

            };
            var result = await gateway.Transaction.SaleAsync(transactionRequest);
            return result;
        }

        private static TransactionLineItemKind? ResolveItemKind(string paymentType)
        {
            if (paymentType.Contains("credit", StringComparison.OrdinalIgnoreCase))
            {
                return TransactionLineItemKind.CREDIT;
            }
            else if (paymentType.Contains("debit", StringComparison.OrdinalIgnoreCase))
            {
                return TransactionLineItemKind.DEBIT;
            }
            else
            {
                return TransactionLineItemKind.UNRECOGNIZED;
            }
        }
    }
}