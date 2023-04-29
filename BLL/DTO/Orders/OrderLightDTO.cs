namespace BLL.DTO.Orders
{
	public class OrderLightDTO
	{
		public string OrderNumber { get; set; }	= string.Empty;
		public int UserId { get; set; }
		public List<GameOrderDTO> Games { get; set; } = new();
		public BillingAddressDTO? BillingAddress { get; set; }
		public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public string Nonce { get; set; } = string.Empty;
		public string PaymentType { get; set; } = string.Empty;
    }
}