namespace API.Models.Filters
{
	public class FilterGameModel
	{
		public string? SearchQuery { get; set; } = string.Empty;
		public List<int>? RegionIds { get; set; } = new();
		public List<int>? PlatformIds { get; set; } = new();
		public List<int>? GenreIds { get; set; } = new ();
		public int DeveloperId { get; set; }
		public int PublisherId { get; set; }
		public DateTime DateFrom { get; set; } = DateTime.MinValue;
		public DateTime DateTo { get; set; } = DateTime.MaxValue;
		public decimal PriceFrom { get; set; } = 0;
		public decimal PriceTo { get; set; } = 1000;
		public bool IsAvailable { get; set; } = true;
		public bool IsDiscounted { get; set; }
		public bool IsHotOffer { get; set; }
	}
}
