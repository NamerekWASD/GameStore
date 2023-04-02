using System.ComponentModel.DataAnnotations;

namespace API.Models.Users
{
	public class LoginModel
	{
		[Required]
		public string Email { get; set; } = string.Empty;
	}
}