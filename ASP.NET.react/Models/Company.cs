namespace ASP.NET.react.Models
{
	public class Company
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<UserModel> Users { get; set; }
	}
}
