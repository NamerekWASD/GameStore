using BLL.DTO.Enums;

namespace BLL.DTO.Filters
{
	public class FilterGameDTO
	{
		public string? SearchQuery { get; set; }
		public List<int> RegionIds { get; set; } = new List<int>();
		public List<int> PlatformIds { get; set; } = new List<int>();
		public List<int> GenreIds { get; set; } = new List<int>();
		public List<int> TagIds { get; set; } = new List<int>();
		public int DeveloperId { get; set; }
		public int PublisherId { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public decimal? PriceFrom { get; set; }
		public decimal? PriceTo { get; set; }
		public bool IsAvailable { get; set; }
		public bool IsDiscounted { get; set; }
		public bool IsHotOffer { get; set; }
		public OrderBy OrderBy { get; set; } = OrderBy.DEFAULT;
	}
}