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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BraintreePaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IBraintreeService _braintreeService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<BraintreePaymentController> _logger;
        private readonly IGameService _gameService;

        public BraintreePaymentController(IOrderService orderService,
            IBraintreeService gateway,
            UserManager<User> userManager,
            ILogger<BraintreePaymentController> logger,
            IGameService gameService)
        {
            _orderService = orderService;
            _braintreeService = gateway;
            _userManager = userManager;
            _logger = logger;
            _gameService = gameService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderLightModel data)
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new NonAuthorizedException("Ви не авторизувались!");
            var orderLight = MapperHelpers.Instance.Map<OrderLightDTO>(data);
            orderLight.UserId = user.Id;

            try
            {
                var orderId = await _orderService.CreateOrder(orderLight);
                orderLight.OrderNumber = orderId;
                var result = await _braintreeService.MakeTransaction(orderLight, user);

                if (!result.IsSuccess())
                {
                    throw new Exception(result.Message);
                }
                await _orderService.CommitChanges();

                return Ok(orderId);
            }
            catch (Exception ex)
            {
                // Якщо сталася помилка, транзакція буде відкатана автоматично
                return BadRequest(string.Format("Сталась помилка: {0}", ex.Message));
            }
        }

        [HttpPost("getClientToken")]
        public async Task<IActionResult> GetClientToken()
        {
            try
            {
                var clientToken = await _braintreeService.GetClientToken();
                return Ok(clientToken);
            }
            catch (Exception ex)
            {
                return BadRequest(string.Format("Сталась помилка: {0}", ex.Message));
            }
        }
    }
}