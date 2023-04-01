using BLL.DTO.GameType;

namespace BLL.DTO.Regions
{
    public class RegionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CopyTypeDTO> CopyTypes { get; set; } = new List<CopyTypeDTO>();
    }
}