using BLL.DTO.Cheques;
using DAL.Entity;
using DAL.Entity.Games;

namespace BLL.DTO
{
    public class ChequeDTO
	{
		public int Id { get; set; }
		public List<SoldCopyDTO>? Games { get; set; }
		public User? Buyer { get; set; }
		public BillingAddressDTO? Bill { get; set; }
		public DateTime? Created { get; set; }
	}
}
