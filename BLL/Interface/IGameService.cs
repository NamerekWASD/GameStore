using BLL.DTO;
using BLL.DTO.Filters;
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
		IAsyncEnumerable<GameDTO> GetGamesByQuery(string query, CancellationToken cancellationToken);
		IAsyncEnumerable<GenreDTO> GetGenres();
		Task<bool> BindImageToGame(int gameId, string fileName, string path);
		Task<IEnumerable<GameDTO>> GetGamesByFilter(FilterGameDTO filter, CancellationToken cancellationToken);
		Task<FilterFormDTO?> GetFilterData(CancellationToken cancellationToken);
	}
}