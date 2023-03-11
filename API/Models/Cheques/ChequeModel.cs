namespace API.Models.Cheques
{
    public class ChequeModel
	{
		public int Id { get; set; }
		public List<SoldCopyModel>? Games { get; set; }
		public DateTime? Created { get; set; }
	}
}
