using BLL.DTO.Filters;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using BLL.DTO.Images;
using BLL.Interface;
using BLL.Tools;
using DAL.Context;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Images;
using DAL.UoW;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
	public class GameService : IGameService
	{

		private readonly IUnitOfWork UoW;
		private readonly ILogger<GameService> _logger;

		public GameService(GameContext context, ILogger<GameService> logger)
		{
			UoW = new UnitOfWork(context);
			_logger = logger;
		}

		public async Task<GameDTO> GetGame(int id)
		{
			return MapperHelper.Instance.Map<GameDTO>(await UoW.Games.GetAsync(id));
		}

		public async IAsyncEnumerable<GameDTO> GetGames()
		{
			await foreach (var game in UoW.Games.GetAll())
			{
				yield return MapperHelper.Instance.Map<GameDTO>(game);
			}
		}

		public async IAsyncEnumerable<GameDTO> GetGames(int[] ids)
		{
			await foreach (var game in UoW.Games.GetAll(item => ids.Contains(item.Id)))
			{
				yield return MapperHelper.Instance.Map<GameDTO>(game);
			}
		}
		public async IAsyncEnumerable<GameDTO> GetGamesByGenre(int genreId)
		{
			await foreach (var game in UoW.Games.GetAll(item => item.Genres.Any(genre => genre.Id == genreId)))
			{
				yield return MapperHelper.Instance.Map<GameDTO>(game);
			}
		}
		public async IAsyncEnumerable<GameDTO> GetGamesByQuery(string query, [EnumeratorCancellation] CancellationToken token)
		{
			await foreach (var game in UoW.Games.GetAll(item => item.Title.Contains(query)).WithCancellation(token))
			{
				yield return MapperHelper.Instance.Map<GameDTO>(game);
			}
		}

		public async Task<IEnumerable<GameDTO>> GetGamesByFilter(FilterGameDTO filter, CancellationToken cancellationToken)
		{
			List<GameDTO> filteredGames = new();
			await foreach (var game in UoW.Games.GetAll().WithCancellation(cancellationToken))
			{
				if (filter.GenreIds is not null &&
					filter.GenreIds.Count is not 0 &&
					filter.GenreIds.Any(item => game.Genres.Any(genre => genre.Id == item)))
				{
					filteredGames.Add(MapperHelper.Instance.Map<GameDTO>(game));
					continue;
				}
				if (filter.PlatformIds is not null &&
					filter.PlatformIds.Count is not 0 &&
					filter.PlatformIds.Any(game.CopyType.Platform.Id.Equals))
				{
					filteredGames.Add(MapperHelper.Instance.Map<GameDTO>(game));
					continue;
				}

				if (filter.CopyTypeNames is not null &&
					filter.CopyTypeNames.Count is not 0 &&
					filter.CopyTypeNames.Any(game.CopyType.Name.Equals))
				{
					filteredGames.Add(MapperHelper.Instance.Map<GameDTO>(game));
					continue;
				}
			}

			if (!filteredGames.Any())
			{
				return new List<GameDTO>();
			}

			filteredGames = filteredGames.Where(game => filter.DeveloperId is 0 || filter.DeveloperId.Equals(game.Developer.Id)).ToList();
			filteredGames = filteredGames.Where(game => filter.PublisherId is 0 || filter.PublisherId.Equals(game.Publisher.Id)).ToList();
			filteredGames = filteredGames.Where(game =>
			{
				if (game.DiscountPrice is null)
				{
					return filter.PriceFrom < game.Price && filter.PriceTo > game.Price;
				}
				else
				{
					return filter.PriceFrom < game.DiscountPrice && filter.PriceTo > game.DiscountPrice;
				}
			}).ToList();

			filteredGames = filteredGames.Where(game => filter.DateFrom < game.Released && filter.DateTo > game.Released).ToList();

			if (!filteredGames.Any())
			{
				return new List<GameDTO>();
			}

			if (filter.IsAvailable)
			{
				filteredGames = filteredGames.Where(game => game.IsAvailable).ToList();
			}

			if (filter.IsDiscounted)
			{
				filteredGames = filteredGames.Where(game => game.DiscountPrice is not null).ToList();
			}

			if (filter.IsHotOffer)
			{
				filteredGames = filteredGames.Where(game => game.IsHotOffer).ToList();
			}

			if (filter.SearchQuery is null)
			{
				return filteredGames;
			}

			return filteredGames.Where(game => game.Title.Contains(filter.SearchQuery));
		}

		public async Task<FilterFormDTO> GetFilterData(CancellationToken token)
		{
			var filter = new FilterFormDTO();

			await foreach (var copyType in UoW.CopyTypes.GetAll())
			{
				filter.CopyTypes.Add(MapperHelper.Instance.Map<CopyTypeDTO>(copyType));
			}

			await foreach (var platform in UoW.Platforms.GetAll())
			{
				filter.Platforms.Add(MapperHelper.Instance.Map<PlatformDTO>(platform));
			}

			await foreach (var genre in UoW.Genres.GetAll())
			{
				filter.Genres.Add(MapperHelper.Instance.Map<GenreDTO>(genre));
			}

			await foreach (var developer in UoW.Developers.GetAll())
			{
				filter.Developers.Add(MapperHelper.Instance.Map<DeveloperDTO>(developer));
			}

			await foreach (var publisher in UoW.Publishers.GetAll())
			{
				filter.Publishers.Add(MapperHelper.Instance.Map<PublisherDTO>(publisher));
			}

			await foreach (var region in UoW.Regions.GetAll())
			{
				filter.AvailableRegions.Add(MapperHelper.Instance.Map<RegionDTO>(region));
			}

			return filter;
		}

		public async Task<GameDTO> AddGame(GameDTO game)
		{
			throw new NotImplementedException();
		}

		public async Task<GameDTO> EditGame(int id, GameDTO game)
		{
			var gameFromDB = await UoW.Games.GetAsync(id);
			gameFromDB.Title = game.Title;
			gameFromDB.Description = game.Description;
			gameFromDB.Price = game.Price;
			gameFromDB.IsAvailable = game.IsAvailable;
			gameFromDB.IsHotOffer = game.IsHotOffer;
			gameFromDB.SoldCopies = game.SoldCopies;
			gameFromDB.DiscountPrice = game.DiscountPrice;
			gameFromDB.Released = game.Released;


			await ProcessGameGenres(game, gameFromDB);
			await ProcessGameImages(game, gameFromDB);
			await ProcessGameDeveloper(game, gameFromDB);
			await ProcessGamePublisher(game, gameFromDB);
			await ProcessGameCopyType(game, gameFromDB);

			return MapperHelper.Instance.Map<GameDTO>(await UoW.Games.ModifyAsync(id, MapperHelper.Instance.Map<Game>(gameFromDB)));
		}

		private async Task ProcessGameGenres(GameDTO game, Game gameFromDB)
		{
			foreach (var item in gameFromDB.Genres.ToList())
			{
				gameFromDB.Genres.Remove(item);
			}
			foreach (var genre in game.Genres.ToList())
			{
				if (genre.Id is 0)
				{
					gameFromDB.Genres.Add(MapperHelper.Instance.Map<Genre>(genre));
					continue;
				}
				else if (genre.Id is not 0)
				{
					gameFromDB.Genres.Add(await UoW.Genres.GetAsync(genre.Id));
					continue;
				}
			}
		}

		private async Task ProcessGameImages(GameDTO game, Game gameFromDB)
		{
			foreach (var image in gameFromDB.Images.ToList())
			{
				gameFromDB.Images.Remove(image);
			}
			foreach (var image in game.Images.ToList())
			{
				if (image.Id is 0)
				{
					gameFromDB.Images.Add(MapperHelper.Instance.Map<Image>(image));
					continue;
				}
				else
				{
					gameFromDB.Images.Add(await UoW.Images.GetAsync(image.Id));
				}
			}
		}

		private async Task ProcessGamePublisher(GameDTO game, Game gameFromDB)
		{
			if (game.PublisherId is not 0 &&
							!game.PublisherId.Equals(gameFromDB.PublisherId))
			{
				gameFromDB.Publisher = await UoW.Publishers.GetAsync(game.PublisherId);
				gameFromDB.PublisherId = gameFromDB.Publisher.Id;
				return;
			}
			if (game.PublisherId is 0)
			{
				gameFromDB.Publisher = await UoW.Publishers.AddAsync(new()
				{
					Id = 0,
					Name = game.Publisher.Name,
				});
				gameFromDB.PublisherId = gameFromDB.Publisher.Id;
			}
		}

		private async Task ProcessGameDeveloper(GameDTO game, Game gameFromDB)
		{
			if (game.DeveloperId is not 0 &&
							!game.DeveloperId.Equals(gameFromDB.DeveloperId))
			{
				gameFromDB.Developer = await UoW.Developers.GetAsync(game.DeveloperId);
				gameFromDB.DeveloperId = gameFromDB.Developer.Id;
				return;
			}
			if (game.DeveloperId is 0)
			{
				gameFromDB.Developer = await UoW.Developers.AddAsync(new()
				{
					Id = 0,
					Name = game.Developer.Name,
				});
				gameFromDB.DeveloperId = gameFromDB.Developer.Id;
			}
		}

		private async Task ProcessGameCopyType(GameDTO game, Game gameFromDB)
		{
			if (game.CopyTypeId is not 0 &&
							!game.CopyTypeId.Equals(gameFromDB.CopyTypeId))
			{
				gameFromDB.CopyType = await UoW.CopyTypes.GetAsync(game.CopyTypeId);
			}
			if(game.CopyTypeId is 0)
			{
				var newCopyType = new CopyType()
				{
					Id = 0,
					Name = game.CopyType.Name,
					AvailableRegions = new(),
				};
				await ProcessCopyType(game.CopyType, newCopyType);

				gameFromDB.CopyType = await UoW.CopyTypes.AddAsync(newCopyType);
				gameFromDB.CopyTypeId = gameFromDB.CopyType.Id;
			}
		}

		private async Task ProcessCopyType(CopyTypeDTO copy, CopyType newCopy)
		{
			if (copy.PlatformId is not 0 &&
				!copy.PlatformId.Equals(newCopy.Id))
			{
				newCopy.Platform = await UoW.Platforms.GetAsync(copy.PlatformId);
				newCopy.PlatformId = newCopy.Platform.Id;
			}
			if (copy.PlatformId is 0)
			{
				newCopy.Platform = new Platform()
				{
					Id = 0,
					Name = copy.Platform.Name
				};
				newCopy.PlatformId = 0;
			}

			foreach (var region in copy.AvailableRegions.ToList())
			{
				if (region.Id is 0)
				{
					newCopy.AvailableRegions.Add(MapperHelper.Instance.Map<Region>(region));
					continue;
				}
				else
				{
					newCopy.AvailableRegions.Add(await UoW.Regions.GetAsync(region.Id));
				}
			}
		}

		public async Task<GameDTO> DeleteGame(int id)
		{
			throw new NotImplementedException();
		}

		public async IAsyncEnumerable<GenreDTO> GetGenres()
		{
			await foreach (var genre in UoW.Genres.GetAll())
			{
				yield return MapperHelper.Instance.Map<GenreDTO>(genre);
			}
		}
		public async IAsyncEnumerable<PublisherDTO> GetPublishers()
		{
			await foreach (var publishers in UoW.Genres.GetAll())
			{
				yield return MapperHelper.Instance.Map<PublisherDTO>(publishers);
			}
		}
		public async IAsyncEnumerable<PlatformDTO> GetPlatforms()
		{
			await foreach (var platform in UoW.Genres.GetAll())
			{
				yield return MapperHelper.Instance.Map<PlatformDTO>(platform);
			}
		}

		public async Task<bool> BindImageToGame(int gameId, string fileName, string path)
		{
			var image = new ImageDTO() { GameId = gameId, Path = path, Name = fileName };
			try
			{
				await UoW.Images.AddAsync(MapperHelper.Instance.Map<Image>(image));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return false;
			}
			return true;
		}
	}
}