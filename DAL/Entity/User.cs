using DAL.Entity.Cheques;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entity
{
    public class User : IdentityUser<int>
	{
		public string ImageURL { get; set; } = string.Empty;
		public virtual List<Cheque>? Cheques { get; set; }
	}
}