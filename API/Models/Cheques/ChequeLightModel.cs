namespace API.Models.Cheques
{
    public class ChequeLightModel
    {
        public string UserEmail { get; set; } = string.Empty;
        public List<GameChequeModel>? Games { get; set; }
        public BillingAddressModel? BillingAddress { get; set; }
    }
}
