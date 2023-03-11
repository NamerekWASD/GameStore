using System.ComponentModel.DataAnnotations;

namespace API.Models
{
	public class RegisterModel
	{
		[Required]
		public string UserName { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		public bool RememberMe { get; set; }
	}
}