using API.Models;
using DAL.Entity;
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
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<User> _signInManager;

		public AccountController(
			UserManager<User> _userManager,
			SignInManager<User> _signInManager,
			ILogger<AccountController> _logger)
		{
			this._signInManager = _signInManager;
			this._logger = _logger;
			this._userManager = _userManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<bool> IsAuthenticated()
		{
			var IsAuthenticated = await Task<bool>.Factory.StartNew(() => User != null
				&& User.Identity != null && User.Identity.IsAuthenticated);
			return IsAuthenticated;
		}
		[HttpGet("login")]
		[AllowAnonymous]
		public IActionResult Login(string ReturnUrl = "")
		{
			return Ok(ReturnUrl);
		}
		[HttpGet("data")]
		[Authorize]
		public async Task<IActionResult> GetUserData()
		{
			var userFromDB = await _userManager.GetUserAsync(User);
			var externalLogin = await _signInManager.GetExternalLoginInfoAsync();
			if (userFromDB is null)
			{
				return BadRequest("User not found");
			}
			var user = new UserModel()
			{
				ImageURL = userFromDB.ImageURL ?? "",
				UserName = userFromDB.UserName ?? "",
				Email = userFromDB.Email ?? "",
				Provider = externalLogin?.LoginProvider ?? ""
			};

			return Ok(user);
		}

		[HttpPost("update")]
		[Authorize]
		public async Task<IActionResult> UpdateUserData([FromBody] UserModel userModel)
		{
			var userFromDB = await _userManager.GetUserAsync(User);
			if (userFromDB is null)
			{
				return BadRequest("User not found");
			}
			userFromDB.Email = userModel.Email;
			userFromDB.UserName = userModel.UserName;
			userFromDB.ImageURL = userModel.ImageURL;

			var result = await _userManager.UpdateAsync(userFromDB);
			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Errors.First().Description);
			}
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginModel login)
		{
			var user = await _userManager.FindByNameAsync(login.UserNameOrEmail)
				?? await _userManager.FindByEmailAsync(login.UserNameOrEmail);
			if (user == null)
			{
				return BadRequest($"Invalid username/E-mail or password!");
			}

			var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
			if (result.Succeeded)
			{
				_logger.LogInformation("User sign in!\n" + login.ToString());
				return Ok();
			}
			return BadRequest($"Invalid username/E-mail or password!");
		}

		[HttpPut("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterModel newUser)
		{
			var user = new User()
			{
				UserName = newUser.UserName,
				Email = newUser.Email,
			};
			var result = await _userManager.CreateAsync(user, newUser.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password.");
				_logger.LogInformation("Registered user: " + newUser.UserName);
				return Ok();
			}
			else
			{
				return BadRequest(result.Errors.First().Description);
			}
		}

		[HttpPost("external-login-callback")]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback([FromBody] ExternalAuthModel external)
		{
			var signInResult = await _signInManager.ExternalLoginSignInAsync(external.LoginProvider,
				external.UserId, isPersistent: external.RememberMe, bypassTwoFactor: true);

			if (signInResult.Succeeded)
			{
				_logger.LogInformation($"User {external?.User?.UserName} signed in!");
				return Ok();
			}
			else
			{
				var email = external.User?.Email;

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
					};

					await _userManager.CreateAsync(user);
				}

				await _userManager.AddLoginAsync(user, new UserLoginInfo(external.LoginProvider, external.UserId, external.User.Email));
				await _signInManager.SignInAsync(user, isPersistent: false);

				return Ok();
			}
		}

		[HttpGet("logout")]
		[Authorize]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			return Ok("User logged out!");
		}
	}
}