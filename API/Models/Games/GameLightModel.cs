using API.Models.Images;

namespace API.Models.Games
{
    public class GameLightModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
		public ImageModel? Image { get; set; }
		public string Platform { get; set; } = string.Empty;
        public string CopyType { get; set; } = string.Empty;
		public string[]? Genres { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsHotOffer { get; set; }
		public int CopyCount { get; set; }
		public int SoldCopies { get; set; }
		public decimal? Price { get; set; }
		public decimal? DiscountPrice { get; set; }
	}
}
