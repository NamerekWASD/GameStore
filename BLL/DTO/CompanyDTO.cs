using DAL.Entity;

namespace BLL.DTO
{
	public class CompanyDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<User> Users { get; set; } = new();

	}
}