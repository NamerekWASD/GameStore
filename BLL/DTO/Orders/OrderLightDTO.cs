
namespace BLL.DTO.Orders
{
    public class OrderLightDTO
    {
		public int UserId { get; set; }
		public List<GameOrderDTO>? Games { get; set; }
		public int BillId { get; set; }
		public BillingAddressDTO? BillingAddress { get; set; }
		public decimal TotalPrice { get; set; }
		public string Nonce { get; set; } = string.Empty;
	}
}
