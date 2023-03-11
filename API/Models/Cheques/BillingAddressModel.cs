namespace API.Models.Cheques
{
    public class BillingAddressModel
	{
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
}
