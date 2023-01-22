using System.Threading.Tasks;
using ASP.NET.react.Models;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET.react.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private static readonly int defaultDelay = 500;

        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<User> _signInManager;
        public AccountController(
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            ILogger<AccountController> _logger,
            ApplicationContext _context)
        {
            this._signInManager = _signInManager;
            this._logger = _logger;
            this._userManager = _userManager;
            /*if (!_context.Users.Any() && initUsers().Result)
			{
				Console.WriteLine("Current users count: " + _context.Users.Count());
			}*/
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> IsAuthenticated()
        {
            var IsAuthenticated = await Task<bool>.Factory.StartNew(() => (User != null)
                && (User.Identity != null) && User.Identity.IsAuthenticated);
            return IsAuthenticated;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            Thread.Sleep(defaultDelay);
            Console.WriteLine(login);
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
                return Ok($"Welcome {user.Name}");
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
            Thread.Sleep(defaultDelay);
            var success = await RegisterUser(newUser);
            if (success)
            {
                return RedirectToAction("Login", "Account",
                    new LoginModel()
                    {
                        UserNameOrEmail = newUser.UserName,
                        Password = newUser.Password,
                        RememberMe = newUser.RememberMe,
                    });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            Thread.Sleep(defaultDelay);

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok("User logged out!");
        }
        private async Task<bool> RegisterUser(RegisterModel loginModel)
        {
            var user = new User()
            {
                UserName = loginModel.UserName,
                Email = loginModel.Email,
                PhoneNumber = loginModel.PhoneNumber,
                Name = loginModel.Name,
                Lastname = loginModel.Lastname
            };
            var result = await _userManager.CreateAsync(user, loginModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                _logger.LogInformation($"Registered user: {loginModel.UserName}");
                return true;
            }
            else
            {
                _logger.LogInformation(result.Errors.ToString());
                return false;
            }
        }
    }
}
