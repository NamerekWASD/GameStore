namespace ASP.NET.react.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public int CompanyId { get; set; }
	}
}
