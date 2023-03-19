
using DAL.Entity.Games;

namespace BLL.DTO.GameType
{
    public class CopyTypeDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int PlatformId { get; set; }
		public PlatformDTO? Platform { get; set; }
		public List<RegionDTO> AvailableRegions { get; set; } = new();
		public List<Game>? Games { get; set; } = new();
	}
}