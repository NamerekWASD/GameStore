using BLL.DTO;
using BLL.DTO.Games;

namespace BLL.Interface
{
	public interface IGameService
	{
		Task<GameDTO> GetGame(int id);

		Task<GameDTO> AddGame(GameDTO game);

		Task<GameDTO> EditGame(int id, GameDTO game);

		Task<GameDTO> DeleteGame(int id);
		IAsyncEnumerable<GameDTO> GetGames();
		IAsyncEnumerable<GameDTO> GetGames(int[] ids);
		IAsyncEnumerable<GameDTO> GetGamesByGenre(int genreId); 

		IAsyncEnumerable<GenreDTO> GetGenres();
	}
}