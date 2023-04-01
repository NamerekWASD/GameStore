namespace DAL.Entity.BillingAddresses
{
    public class BillingAddress
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string CountryCodeAlpha2 { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
    }
}