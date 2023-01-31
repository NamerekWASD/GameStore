using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ASP.NET.react.Models;
using ASP.NET.react.Utility;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NET.react.Controllers
{
	[ApiController]
	[Route("[controller]")]
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
			var IsAuthenticated = await Task<bool>.Factory.StartNew(() => (User != null)
				&& (User.Identity != null) && User.Identity.IsAuthenticated);
			return IsAuthenticated;
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
			var user = new UserModel()
			{
				UserName = userFromDB.UserName ?? "",
				Name = userFromDB.Name ?? "",
				Lastname = userFromDB.Lastname ?? "",
				Email = userFromDB.Email ?? "",
				PhoneNumber = userFromDB.PhoneNumber ?? "",
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
			userFromDB.PhoneNumber = userModel.PhoneNumber;
			userFromDB.Name = userModel.Name;
			userFromDB.Lastname = userModel.Lastname;

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


		[HttpGet("login")]
		[AllowAnonymous]
		public IActionResult Login(string? returnUrl = null)
		{
			return Ok();
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
				_logger.LogInformation("User sign in!");
				return Ok();
			}
			else
			{
				return BadRequest($"Invalid username/E-mail or password!");
			}
		}
		[HttpPut("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterModel newUser)
		{
			var user = new User()
			{
				UserName = newUser.UserName,
				Email = newUser.Email,
				PhoneNumber = newUser.PhoneNumber,
				Name = newUser.Name,
				Lastname = newUser.Lastname
			};
			var result = await _userManager.CreateAsync(user, newUser.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password.");
				_logger.LogInformation($"Registered user: {newUser.UserName}");
				return Ok();
			}
			else
			{
				return BadRequest(result.Errors.First().Description);
			}

		}

		[HttpPost("external-auth")]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalAuthorization([FromBody] ExternalAuthModel externalModel)
		{
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(externalModel.LoginProvider, Request.Path);
			properties.AllowRefresh = true;
			return Challenge(properties, externalModel.LoginProvider);
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
