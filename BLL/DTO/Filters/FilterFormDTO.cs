using BLL.DTO.Developers;
using BLL.DTO.GameType;
using BLL.DTO.Gernres;
using BLL.DTO.Images;
using BLL.DTO.Platforms;
using BLL.DTO.Publishers;
using BLL.DTO.Regions;
using BLL.DTO.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Filters
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
