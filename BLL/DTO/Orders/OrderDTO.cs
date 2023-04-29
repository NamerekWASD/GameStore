using DAL.Entity;

namespace BLL.DTO.Orders
{
	public class OrderDTO
	{
		public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public List<SoldCopyDTO>? Copies { get; set; }
		public int BuyerId { get; set; }
		public User? Buyer { get; set; }
		public int BillingAddressId { get; set; }
		public BillingAddressDTO? Bill { get; set; }
		public DateTime Created { get; set; }
		public decimal Total { get; set; }
	}
}