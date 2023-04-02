namespace API.Models.Users
{
	public class ExternalAuthModel
	{
		public string UserId { get; set; } = string.Empty;
		public string LoginProvider { get; set; } = string.Empty;
		public bool RememberMe { get; set; }
		public UserModel User { get; set; } = new();
	}
}