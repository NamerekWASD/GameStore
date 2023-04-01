using DAL.Entity.Copies;
using DAL.Entity.Orders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity.SoldCopies
{
    public class SoldCopy
    {
        public int Id { get; set; }
        public int CopyId { get; set; }
        public virtual Copy? Copy { get; set; }
        public int OrderId { get; set; }
        public virtual Order? Orders { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
