namespace API.Models
{
	public class OrderLightModel
	{
		public string UserEmail { get; set; } = string.Empty;
		public List<GameOrderModel>? Games { get; set; }
		public int BillingAddressId { get; set; }
		public BillingAddressModel? BillingAddress { get; set; }
		public decimal TotalPrice { get; set; }
		public string Nonce { get; set; } = string.Empty;
		public string PaymentType { get; set; } = string.Empty;
	}
}