namespace API.Models.Filters
{
	public class FilterGameModel
	{
		public string? SearchQuery { get; set; }
		public List<string>? CopyTypeNames { get; set; }
		public List<int>? PlatformIds { get; set; }
		public List<int>? GenreIds { get; set; }
		public int DeveloperId { get; set; }
		public int PublisherId { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public decimal PriceFrom { get; set; }
		public decimal PriceTo { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsDiscounted { get; set; }
		public bool IsHotOffer { get; set; }
	}
}
