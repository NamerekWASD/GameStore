using DAL.Entity;

namespace ASP.NET.react.Models
{
	public class CompanyModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<User> Users { get; set; } = new List<User>();
	}
}
