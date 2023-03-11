using API.Models.Cheques;
using BLL.DTO.Cheques;
using BLL.Interface;
using BLL.Service;
using DAL.Context;
using DAL.Entity;
using DAL.Entity.Cheques;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ChequeController : ControllerBase
	{
		private readonly IChequeService _chequeService;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<ChequeController> _logger;

		public ChequeController(GameContext context, UserManager<User> userManager, ILogger<ChequeController> logger)
		{
			_chequeService = new ChequeService(context);
			_userManager = userManager;
			_logger = logger;
		}
		[HttpGet]
		public IActionResult GetAll(CancellationToken cancellationToken)
		{
			return Ok(GetCheques());
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			return Ok();
		}

		[HttpPost("create")]
		[AllowAnonymous]
		public async Task<IActionResult> Post([FromBody] ChequeLightModel data)
		{
			var user = await _userManager.GetUserAsync(User);
			if(user == null)
			{
				var result = await _userManager.CreateAsync(new User()
				{
					Email = data.UserEmail,
				});
                if (result.Succeeded)
                {
					user = await _userManager.GetUserAsync(User);
				}
				else
				{
					return BadRequest("Не вдалось створити нового користувача.");
				}
            }
			var userId = user.Id;
			var chequelight = MapperHelpers.Instance.Map<ChequeLightDTO>(data);
			chequelight.UserId = userId;
			var cheque = await _chequeService.CreateCheque(chequelight);
			return Ok(MapperHelpers.Instance.Map<ChequeModel>(cheque));
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] string value)
		{
			return Ok();
		}

		private async IAsyncEnumerable<ChequeModel> GetCheques()
		{
			foreach(var cheque in (await _userManager.GetUserAsync(User)).Cheques)
			{
				yield return MapperHelpers.Instance.Map<ChequeModel>(cheque);
			}
		}
	}
}
