
namespace API.Models.Filters
{
    public class FilterFormDataModel
	{
		public List<CopyTypeModel>? CopyTypes { get; set; }
		public List<PlatformModel>? Platforms { get; set; }
		public List<GenreModel>? Genres { get; set; }
		public List<TagModel>? Tags { get; set; }
		public List<DeveloperModel>? Developers { get; set; }
		public List<PublisherModel>? Publishers { get; set; }
		public List<RegionModel>? AvailableRegions { get; set; }
		public List<ImageTypeModel>? ImageTypes { get; set; }
	}
}