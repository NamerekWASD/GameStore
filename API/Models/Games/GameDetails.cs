using API.Models.Images;

namespace API.Models.Games
{
    public class GameDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
		public List<ImageModel>? Images { get; set; }
		public string Description { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
		public string Developer { get; set; } = string.Empty;
		public string[]? Regions { get; set; }
        public DateOnly Released { get; set; }
		public string Platform { get; set; } = string.Empty;
        public string CopyType { get; set; } = string.Empty;
        public string[]? Genres { get; set; }
		public string[]? Tags { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsHotOffer { get; set; }
		public int CopyCount { get; set; }
        public decimal Price { get; set; }
		public decimal? DiscountPrice { get; set; }
		public int SoldCopies { get; set; }
	}
}
