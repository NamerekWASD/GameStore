using System.ComponentModel.DataAnnotations;

namespace ASP.NET.react.Models
{
	public class RegisterModel
	{
		[Required]
		public string UserName { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
		[Required]
		public string Name { get; set; } = string.Empty;
		public string Lastname { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
		[Required]
		public bool RememberMe { get; set; }
	}
}
