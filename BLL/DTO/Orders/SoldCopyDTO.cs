namespace BLL.DTO.Orders
{
	public class SoldCopyDTO
	{
		public int Id { get; set; }
		public int CopyId { get; set; }
		public CopyDTO? Copy { get; set; }
		public int OrderId { get; set; }
		public OrderDTO? Order { get; set; }
		public decimal Price { get; set; }
	}
}