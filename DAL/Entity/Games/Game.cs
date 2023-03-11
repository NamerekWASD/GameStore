using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entity.GameType;

namespace DAL.Entity.Games
{
    public class Game
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string ImageURL { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public virtual List<Genre>? Genres { get; set; }
		public virtual Developer? Developer { get; set; }
		public virtual Publisher? Publisher { get; set; }
		public virtual CopyType? CopyType { get; set; }
		public virtual List<Copy>? Copies { get; set; }
		public DateTime Released { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
		public int Sold { get; set; }
	}
}