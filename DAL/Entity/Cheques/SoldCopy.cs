using DAL.Entity.Games;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity.Cheques
{
    public class SoldCopy
    {
        public int Id { get; set; }
        public int CopyId { get; set; }
        public virtual Copy? Copy { get; set; }
        public int ChequeId { get; set; }
        public virtual Cheque? Cheque { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
