namespace DAL.Entity.Cheques
{
    public class Cheque
    {
        public int Id { get; set; }
        public virtual List<SoldCopy>? Games { get; set; }
        public virtual User? Buyer { get; set; }
        public virtual BillingAddress? Bill { get; set; }
        public DateTime? Created { get; set; }

    }
}
