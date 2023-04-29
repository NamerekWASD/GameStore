namespace API.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public List<SoldCopyModel>? Copies { get; set; }
		public string Created { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}