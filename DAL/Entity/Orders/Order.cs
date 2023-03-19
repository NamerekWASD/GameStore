
namespace DAL.Entity.Orders
{
	public class Order
	{
		public int Id { get; set; }
		public virtual List<SoldCopy>? Copies { get; set; }
		public virtual User? Buyer { get; set; }
		public int BillId { get; set; }
		public virtual BillingAddress? Bill { get; set; }
		public DateTime Created { get; set; }

	}
}
