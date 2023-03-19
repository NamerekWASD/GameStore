using BLL.DTO.Games;
using BLL.DTO.GameType;
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
		public List<DeveloperDTO> Developers { get; set; } = new();
		public List<PublisherDTO> Publishers { get; set; } = new();
		public List<RegionDTO> AvailableRegions { get; set; } = new();
	}
}
