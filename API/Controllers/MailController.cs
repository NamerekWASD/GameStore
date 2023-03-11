using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MailService.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MailController : ControllerBase
	{
		private readonly IMailService mailService;

		public MailController(IMailService mailService)
		{
			this.mailService = mailService;
		}
		[HttpPost("send")]
		public async Task<IActionResult> SendMail([FromForm] MailRequest request)
		{
			try
			{
				await mailService.SendEmailAsync(request);
				return Ok();
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}