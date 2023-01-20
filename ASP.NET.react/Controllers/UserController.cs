using ASP.NET.react.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET.react.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : Controller
	{
		private static readonly IEnumerable<UserModel> userModels = new UserModel[]
		{
			new(){ Id = 1, UserName="Namerek", Name = "Nykolai", Email = "someEmail@gmail.com", CompanyId = 1 },
			new(){ Id = 2, UserName="asd",  Name = "Maria", Email = "Maria@gmail.com", CompanyId = 2 },
			new(){ Id = 3, UserName="gfd",  Name = "Maxim", Email = "Maxim@gmail.com", CompanyId = 1 },
			new(){ Id = 4, UserName="bgd",  Name = "Tomas", Email = "Tomas@gmail.com", CompanyId = 2 },
			new(){ Id = 5, UserName="rew",  Name = "Valentina", Email = "Valentina@gmail.com", CompanyId = 1 },
			new(){ Id = 6, UserName="vcb",  Name = "Katerina", Email = "Katerina@gmail.com", CompanyId = 1 },
			new(){ Id = 7, UserName="ytr",  Name = "Anatoliy", Email = "Anatoliy@gmail.com", CompanyId = 2 },
			new(){ Id = 8, UserName="jgh",  Name = "Bogdan", Email = "Bogdan@gmail.com", CompanyId = 1 },
			new(){ Id = 9, UserName="nvb",  Name = "Arsen", Email = "Arsen@gmail.com", CompanyId = 2 },
			new(){ Id = 10, UserName="zxc",  Name = "Olexander", Email = "Olexander@gmail.com", CompanyId = 1 }
		};
		[HttpGet]
		public UserModel[] Get()
		{
			return userModels.ToArray();
		}

		[HttpGet("{userName}")]
		public UserModel[] Get(string username)
		{
			Thread.Sleep(1000);
			return userModels.Where((user) => user.UserName == username).ToArray();
		}
		[Route("/Login")]
		[HttpPost("{username, password}")]
		public UserModel[] Login(string username)
		{
			Thread.Sleep(1000);
			return userModels.Where((user) => user.UserName == username).ToArray();
		}
	}
}
