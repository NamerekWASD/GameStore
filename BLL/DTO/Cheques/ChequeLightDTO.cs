
namespace BLL.DTO.Cheques
{
    public class ChequeLightDTO
    {
		public int UserId { get; set; }
		public List<GameChequeDTO>? Games { get; set; }
		public BillingAddressDTO? BillingAddress { get; set; }
	}
}
