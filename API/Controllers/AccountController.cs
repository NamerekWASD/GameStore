using API.Models.Users;
using BLL.Service.Mails;
using BLL.Tools;
using DAL.Entity;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
#pragma warning disable IDE0052 // Remove unread private members
		private readonly ILogger<AccountController> _logger;
#pragma warning restore IDE0052 // Remove unread private members
		private readonly SignInManager<User> _signInManager;
		private readonly IMailService _mailService;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly RoleManager<IdentityRole<int>> _roleManager;

		public AccountController(
			UserManager<User> _userManager,
			SignInManager<User> _signInManager,
			ILogger<AccountController> _logger,
			IWebHostEnvironment _appEnvironment,
			IMailService _mailService,
			RoleManager<IdentityRole<int>> _roleManager)
		{
			this._signInManager = _signInManager;
			this._logger = _logger;
			this._userManager = _userManager;
			this._appEnvironment = _appEnvironment;
			this._mailService = _mailService;
			this._roleManager = _roleManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public bool IsAuthenticated()
		{
			return User != null && User.Identity != null && User.Identity.IsAuthenticated && _signInManager.IsSignedIn(User);
		}

		[HttpGet("login")]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return Ok("Спершу авторизуйтесь");
		}

		[HttpGet("accessDenied")]
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return Ok("У вас недостатньо прав...");
		}

		[HttpGet(Constants.MANAGER)]
		[Authorize(Constants.MANAGER)]
		public IActionResult IsManager()
		{
			return Ok();
		}

		[HttpGet(Constants.ADMINISTRATOR)]
		[Authorize(Constants.ADMINISTRATOR)]
		public IActionResult IsAdministrator()
		{
			return Ok();
		}

		[HttpGet("data")]
		[Authorize]
		public async Task<IActionResult> GetUserData()
		{
			var userFromDB = await _userManager.GetUserAsync(User);

			if (userFromDB is null)
			{
				return BadRequest("User not found");
			}
			var user = new UserModel
			{
				ImageURL = userFromDB.ImageURL ?? "",
				UserName = userFromDB.UserName ?? "",
				FirstName = userFromDB.FirstName ?? "",
				LastName = userFromDB.LastName ?? "",
				Email = userFromDB.Email ?? "",
			};

			return Ok(user);
		}

		[HttpPost("update")]
		[Authorize]
		public async Task<IActionResult> UpdateUserData([FromBody] UserModel userModel)
		{
			var userFromDB = await _userManager.GetUserAsync(User) ?? throw new NotFoundException("Користувача не знайдено");
			userFromDB.Email = userModel.Email;
			userFromDB.FirstName = userModel.FirstName;
			userFromDB.LastName = userModel.LastName;
			userFromDB.UserName = userModel.UserName;
			userFromDB.ImageURL = userModel.ImageURL;

			var result = await _userManager.UpdateAsync(userFromDB);
			if (result.Succeeded)
			{
				return Ok("Готово!");
			}
			else
			{
				return BadRequest(result.Errors.First().Description);
			}
		}

		[HttpPost("authorize")]
		public async Task<IActionResult> Auth([FromBody] LoginModel login)
		{
			await CheckRoleExists();
			var user = await _userManager.FindByEmailAsync(login.Email);
			if (user == null)
			{
				user = new User() { Email = login.Email, UserName = login.Email };
				var result = await _userManager.CreateAsync(user);
				if (!result.Succeeded) return BadRequest(result.Errors.First().Description);
			}
			if (_appEnvironment.IsDevelopment())
			{
				await _userManager.AddToRoleAsync(user, Constants.ADMINISTRATOR);
			}
			try
			{
				await _mailService.CreateAndSendConfirmationCode(user);
			}
			catch (Exception ex)
			{
				BadRequest(ex.Message);
			}

			return Ok();
		}

		private async Task CheckRoleExists()
		{
			if (!await _roleManager.RoleExistsAsync(Constants.ADMINISTRATOR))
			{
				var role = new IdentityRole<int>
				{
					Name = Constants.ADMINISTRATOR
				};
				await _roleManager.CreateAsync(role);
			}

			if (!await _roleManager.RoleExistsAsync(Constants.MANAGER))
			{
				var role = new IdentityRole<int>
				{
					Name = Constants.MANAGER
				};
				await _roleManager.CreateAsync(role);
			}
		}

		[HttpPost("confirm")]
		public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmationModel confirmationModel)
		{
			var user = await _userManager.FindByEmailAsync(confirmationModel.Email);
			if (user == null)
			{
				return BadRequest("Error");
			}
			if (confirmationModel.Code != user.ConfirmationCode)
			{
				return BadRequest("Невірний код");
			}
			user.ConfirmationCode = string.Empty;
			user.EmailConfirmed = true;
			await _signInManager.SignInAsync(user, isPersistent: true);
			return Ok("Ok");
		}

		[HttpPost("external-login-callback")]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback([FromBody] ExternalAuthModel external)
		{
			var signInResult = await _signInManager.ExternalLoginSignInAsync(external.LoginProvider,
				external.UserId, isPersistent: external.RememberMe, bypassTwoFactor: true);

			if (signInResult.Succeeded)
			{
				return Ok("Ok");
			}
			var email = external.User.Email;

			if (email == null)
			{
				return BadRequest($"Email claim not received from: {external.LoginProvider}");
			}

			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
				user = new User
				{
					ImageURL = external.User.ImageURL,
					UserName = external.User.UserName,
					Email = external.User.Email,
					EmailConfirmed = true,
				};

				await _userManager.CreateAsync(user);
			}

			await _userManager.AddLoginAsync(user, new UserLoginInfo(external.LoginProvider, external.UserId, external.LoginProvider));
			await _signInManager.SignInAsync(user, isPersistent: true);

			return Ok();
		}

		[HttpGet("logout")]
		[Authorize]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return Ok("Готово!");
		}
	}
}