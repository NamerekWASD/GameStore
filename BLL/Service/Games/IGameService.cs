using BLL.DTO.Filters;
using BLL.DTO.Games;
using BLL.DTO.Images;
using BLL.DTO.Lists;

namespace BLL.Service.Games
{
    public interface IGameService
    {
        Task<GameDTO> GetGame(int id);
        Task<GameDTO> AddGame(GameDTO game);
        Task<GameDTO?> EditGame(GameDTO game);
        Task<bool> DeleteGameWithSerializedData(int id, string serialized);
        Task<GameListDTO> GetGamesWithPagination(int page, CancellationToken cancellationToken);
        Task<GameListDTO> GetGamesByFilterWithPagination(FilterGameDTO filterGameDTO, int page, CancellationToken cancellationToken);
        IAsyncEnumerable<GameDTO> GetGames(int[] ids, CancellationToken cancellationToken);
        Task<ImageDTO?> BindImageToGame(int gameId, ImageFormDTO model);
        Task<FilterFormDTO> GetFilterData(CancellationToken cancellationToken);
		Task<bool> DeleteGame(int id);
	}
}