using API.Models.Orders;
using BLL.DTO.Orders;
using BLL.Interface;
using Braintree;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IBraintreeService _braintreeService;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<OrderController> _logger;

		public OrderController(IOrderService orderService,
			IBraintreeService gateway,
			UserManager<User> userManager, 
			ILogger<OrderController> logger)
		{
			_orderService = orderService;
			_braintreeService = gateway;
			_userManager = userManager;
			_logger = logger;
		}
		[HttpGet]
		public IActionResult GetAll(CancellationToken cancellationToken)
		{
			return Ok(GetOrders(cancellationToken));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return BadRequest("Користувача не знайдено.");
			}
			var order = await _orderService.GetOrder(user.Id, id);
			if (order == null)
			{
				return BadRequest("Замовлення не знайдено.");
			}

			return Ok(MapperHelpers.Instance.Map<OrderModel>(order));
		}
		[HttpGet("last-bill")]
		public async Task<IActionResult> GetLastBillingAddress()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user is null || user.Orders is null || user.Orders.Count is 0)
			{
				return Ok(0);
			}

			return Ok(user.Orders.LastOrDefault().Bill);
		}
		[HttpPost("create")]
		public async Task<IActionResult> CreateOrder([FromBody] OrderLightModel data)
		{
			var orderlight = MapperHelpers.Instance.Map<OrderLightDTO>(data);
			await _orderService.CalculateTotalPrice(orderlight);
			var gateway = _braintreeService.GetGateway();
			var request = new TransactionRequest
			{

				BillingAddress = MapperHelpers.Instance.Map<AddressRequest>(data.BillingAddress),
				Amount = orderlight.TotalPrice,
				CurrencyIsoCode = "USD",
				PaymentMethodNonce = data.Nonce,
				Options = new TransactionOptionsRequest
				{
					SubmitForSettlement = true
				}
			};

			Result<Transaction> result = gateway.Transaction.Sale(request);
			if (!result.IsSuccess())
			{
				return BadRequest(result.Message);
			}
			var user = await _userManager.GetUserAsync(User);
			if (user is null)
			{
				user = await _userManager.FindByEmailAsync(data.UserEmail);
				if (user is null) return BadRequest("Користувача не знайдено");
			}
			
			orderlight.UserId = user.Id;
			var orderId = await _orderService.CreateOrder(orderlight);
			return Ok(orderId);
		}
		private async IAsyncEnumerable<OrderModel> GetOrders([EnumeratorCancellation] CancellationToken cancellation)
		{
			var user = await _userManager.GetUserAsync(User);
			await foreach (var order in _orderService.GetOrders(user.Id).WithCancellation(cancellation))
			{
				yield return MapperHelpers.Instance.Map<OrderModel>(order);
			}
		}
	}
}
