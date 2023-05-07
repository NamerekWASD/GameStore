namespace API.Models.Games
{
    public class GameModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public List<ImageModel>? Images { get; set; }
		public string Description { get; set; } = string.Empty;
		public List<GenreModel>? Genres { get; set; }
		public List<TagModel>? Tags { get; set; }
		public int DeveloperId { get; set; }
		public DeveloperModel? Developer { get; set; }
		public int PublisherId { get; set; }
		public PublisherModel? Publisher { get; set; }
		public int? CopyTypeId { get; set; }
		public CopyTypeModel? CopyType { get; set; }
		public DateTime? Released { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsHotOffer { get; set; }
		public decimal? Price { get; set; }
		public decimal? DiscountPrice { get; set; }
		public int SoldCopies { get; set; }
	}
}