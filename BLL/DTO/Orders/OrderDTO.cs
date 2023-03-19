using DAL.Entity;

namespace BLL.DTO.Orders
{
    public class OrderDTO
	{
		public int Id { get; set; }
		public List<SoldCopyDTO>? Copies { get; set; }
		public User? Buyer { get; set; }
		public int BillId { get; set; }
		public BillingAddressDTO? Bill { get; set; }
		public DateTime Created { get; set; }
	}
}
