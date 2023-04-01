using BLL.DTO.Games;

namespace BLL.DTO.Gernres
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GameDTO> Games { get; set; } = new();
    }
}