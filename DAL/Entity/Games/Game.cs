using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entity.Copies;
using DAL.Entity.Developers;
using DAL.Entity.GameType;
using DAL.Entity.Genres;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Publishers;
using DAL.Entity.Tags;

namespace DAL.Entity.Games
{
    public class Game
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public virtual List<Image> Images { get; set; } = new();
		public string Description { get; set; } = string.Empty;
		public virtual List<Genre> Genres { get; set; } = new();
		public virtual List<Tag> Tags { get; set; } = new();
		public int DeveloperId { get; set; }
		public virtual Developer? Developer { get; set; }
		public int PublisherId { get; set; }
		public virtual Publisher? Publisher { get; set; }
		public int? CopyTypeId { get; set; }
		public virtual CopyType? CopyType { get; set; }
		public virtual List<Copy> Copies { get; set; } = new();
		public DateTime? Released { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsHotOffer { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? Price { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? DiscountPrice { get; set; }
		public int SoldCopies { get; set; }
		public virtual List<GameSubscription> Subscriptions { get; set; } = new();
	}
}