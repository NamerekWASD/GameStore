using BLL.DTO;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using BLL.Interface;
using BLL.Tools;
using DAL.Context;
using DAL.UoW;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
	public class GameService : IGameService
	{

		private readonly IUnitOfWork UoW;

		public GameService(GameContext context)
		{
			UoW = new UnitOfWork(context);
		}

		public async Task<GameDTO> GetGame(int id)
		{
			return Mapper.Instance.Map<GameDTO>(await UoW.Games.GetAsync(id));
		}

		public async IAsyncEnumerable<GameDTO> GetGames()
		{
			await foreach (var game in UoW.Games.GetAll())
			{
				yield return Mapper.Instance.Map<GameDTO>(game);
			}
		}

		public async IAsyncEnumerable<GameDTO> GetGames(int[] ids)
		{
			await foreach (var game in UoW.Games.GetAll(item => ids.Contains(item.Id)))
			{
				yield return Mapper.Instance.Map<GameDTO>(game);
			}
		}
		public async IAsyncEnumerable<GameDTO> GetGamesByGenre(int genreId)
		{
			await foreach (var game in UoW.Games.GetAll(item => item.Genres.Any(genre => genre.Id == genreId)))
			{
				yield return Mapper.Instance.Map<GameDTO>(game);
			}
		}

		public async Task<GameDTO> AddGame(GameDTO game)
		{
			throw new NotImplementedException();
		}

		public async Task<GameDTO> EditGame(int id, GameDTO game)
		{
			throw new NotImplementedException();
		}

		public async Task<GameDTO> DeleteGame(int id)
		{
			throw new NotImplementedException();
		}

		public async IAsyncEnumerable<GenreDTO> GetGenres()
		{
			await foreach (var genre in UoW.Genres.GetAll())
			{
				yield return Mapper.Instance.Map<GenreDTO>(genre);
			}
		}
		public async IAsyncEnumerable<PublisherDTO> GetPublishers()
		{
			await foreach (var publishers in UoW.Genres.GetAll())	
			{
				yield return Mapper.Instance.Map<PublisherDTO>(publishers);
			}
		}
		public async IAsyncEnumerable<PlatformDTO> GetPlatforms()
		{
			await foreach (var platform in UoW.Genres.GetAll())
			{
				yield return Mapper.Instance.Map<PlatformDTO>(platform);
			}
		}
	}
}