using API.Models;
using API.Tools;
using BLL.DTO.Orders;
using BLL.Service.BrainTree;
using BLL.Service.Games;
using BLL.Service.Orders;
using DAL.Entity;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IGameService _gameService;

        public OrderController(IOrderService orderService,
            IBraintreeService gateway,
            UserManager<User> userManager,
            ILogger<OrderController> logger,
            IGameService gameService)
        {
            _orderService = orderService;
            _braintreeService = gateway;
            _userManager = userManager;
            _logger = logger;
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult GetAll(CancellationToken cancellationToken)
        {
            return Ok(GetOrders(cancellationToken));
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> Get(string orderNumber)
        {
            OrderDTO? order;
            try
            {
                var user = await _userManager.GetUserAsync(User);
                order = await _orderService.GetOrder(user?.Id ?? 0, orderNumber);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
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

            return Ok(user.Orders.LastOrDefault()?.Bill ?? null);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderLightModel data)
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new NonAuthorizedException("Ви не авторизувались!");
            var orderLight = MapperHelpers.Instance.Map<OrderLightDTO>(data);

            orderLight.UserId = user.Id;

            try
            {
                // Створення чеку
                var orderId = await _orderService.CreateOrder(orderLight);

                orderLight.OrderNumber = orderId;
                // Проведення оплати
                var result = await _braintreeService.MakeTransaction(orderLight, user);

                if (!result.IsSuccess())
                {
                    throw new Exception(result.Message);
                }

                // Якщо операції успішні, то зберігаємо дані в контролері

                await _orderService.CommitChanges();

                return Ok(orderId);
            }
            catch (Exception ex)
            {
                // Якщо сталася помилка, транзакція буде відкатана автоматично
                return BadRequest(string.Format("Сталась помилка: {0}", ex.Message));
            }
        }

        private async IAsyncEnumerable<OrderModel> GetOrders([EnumeratorCancellation] CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new NonAuthorizedException();
            await foreach (var order in _orderService.GetOrders(user.Id).WithCancellation(cancellation))
            {
                yield return MapperHelpers.Instance.Map<OrderModel>(order);
            }
        }
    }
}