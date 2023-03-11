using BLL.DTO.Games;

namespace BLL.DTO.Cheques
{
    public class SoldCopyDTO
    {
        public int Id { get; set; }
        public int CopyId { get; set; }
        public CopyDTO? Copy { get; set; }
		public int ChequeId { get; set; }
		public ChequeDTO? Cheque { get; set; }
		public decimal Price { get; set; }
	}
}
