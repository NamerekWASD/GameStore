using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity
{
	public class Order
	{
		public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public virtual List<SoldCopy>? Copies { get; set; }
		public virtual User? Buyer { get; set; }
		public int BillingAddressId { get; set; }
		public virtual BillingAddress? Bill { get; set; }
		public DateTime Created { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }
	}
}