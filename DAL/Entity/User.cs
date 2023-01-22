
using Microsoft.AspNetCore.Identity;

namespace DAL.Entity
{
	public class User : IdentityUser<int>
	{
		public string Name { get; set; } = string.Empty;
		public string Lastname { get; set; } = string.Empty;
		public int CompanyId { get; set; }
	}
}