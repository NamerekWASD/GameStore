namespace API.Models.Orders
{
	public class OrderModel
	{
		public int Id { get; set; }
		public List<SoldCopyModel>? Copies { get; set; }
		public string Created { get; set; } = string.Empty;
	}
}