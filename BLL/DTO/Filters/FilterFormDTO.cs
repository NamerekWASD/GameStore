
namespace BLL.DTO
{
	public class FilterFormDTO
	{
		public List<CopyTypeDTO> CopyTypes { get; set; } = new();
		public List<PlatformDTO> Platforms { get; set; } = new();
		public List<GenreDTO> Genres { get; set; } = new();
		public List<TagDTO> Tags { get; set; } = new();
		public List<DeveloperDTO> Developers { get; set; } = new();
		public List<PublisherDTO> Publishers { get; set; } = new();
		public List<RegionDTO> AvailableRegions { get; set; } = new();
		public List<ImageTypeDTO> ImageTypes { get; set; } = new();
	}
}