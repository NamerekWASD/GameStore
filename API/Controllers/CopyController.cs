using API.Models.Copies;
using BLL.Service.Copies;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CopyController : ControllerBase
	{
		private readonly ICopyService _copyService;

		public CopyController(ICopyService copyService)
		{
			_copyService = copyService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateCopy([FromBody] CopyModel copy)
		{
			var result = await _copyService.CreateCopy(copy.Data, copy.GameId);
			if (result)
			{
				return Ok();
			}
			return BadRequest();
		}
	}
}