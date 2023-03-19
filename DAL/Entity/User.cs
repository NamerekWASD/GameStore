using DAL.Entity.Orders;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entity
{
	public class User : IdentityUser<int>
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? ImageURL { get; set; }
		public virtual List<Order>? Orders { get; set; }
		public string? ConfirmationCode { get; set; }
	}
}