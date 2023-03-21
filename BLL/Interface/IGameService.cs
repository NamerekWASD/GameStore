using BLL.DTO;
using BLL.DTO.Filters;
using BLL.DTO.Games;
using BLL.DTO.Images;

namespace BLL.Interface
{
	public interface IGameService
	{
		Task<GameDTO> GetGame(int id);
		Task<GameDTO> AddGame(GameDTO game);
		Task<GameDTO> EditGame(int id, GameDTO game);
		Task<bool> DeleteGame(int id, string serialized);
		IAsyncEnumerable<GameDTO> GetGames();
		IAsyncEnumerable<GameDTO> GetGames(int[] ids);
		IAsyncEnumerable<GameDTO> GetGamesByGenre(int genreId);
		IAsyncEnumerable<GameDTO> GetGamesByQuery(string query, CancellationToken cancellationToken);
		IAsyncEnumerable<GenreDTO> GetGenres();
		Task<ImageDTO?> BindImageToGame(int gameId, ImageFormDTO model);
		Task<IEnumerable<GameDTO>> GetGamesByFilter(FilterGameDTO filter, CancellationToken cancellationToken);
		Task<FilterFormDTO?> GetFilterData(CancellationToken cancellationToken);
	}
}