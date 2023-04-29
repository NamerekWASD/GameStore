using BLL.DTO;
using BLL.Service.Mails;
using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using DAL.Managers;
using DAL.UoW;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using UnitsOfWork.Interfaces;

namespace BLL.Service.Games
{
	public class GameService : IGameService
	{
		private readonly IUnitOfWork UoW;
#pragma warning disable IDE0052 // Remove unread private members
		private readonly ILogger<GameService> _logger;
#pragma warning restore IDE0052 // Remove unread private members
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly ISubscriptionService _subscriptionService;
		private readonly IServer _server;

		public GameService(IUnitOfWork unitOfWork,
			IWebHostEnvironment _appEnvironment,
			ILogger<GameService> logger,
			ISubscriptionService subscriptionService,
			IServer server)
		{
			UoW = unitOfWork;
			this._appEnvironment = _appEnvironment;
			_logger = logger;
			_subscriptionService = subscriptionService;
			_server = server;
		}

		public GameService(GameContext context,
			IWebHostEnvironment _appEnvironment,
			ILogger<GameService> logger,
			ISubscriptionService subscriptionService,
			IServer server)
		{
			UoW = new UnitOfWork(context);
			this._appEnvironment = _appEnvironment;
			_logger = logger;
			_subscriptionService = subscriptionService;
			_server = server;
		}

		public async Task<GameDTO> GetGame(int id)
		{
			return MapperHelpers.Instance.Map<GameDTO>(await UoW.Games.GetAsync(id));
		}

		public async Task<GameListDTO> GetGamesWithPagination(int page, CancellationToken cancellationToken)
		{
			var gamesQuery = TakeRange(await UoW.Games.GetAllAsync(cancellationToken), page, out int totalCount, out bool isMax);
			var list = new GameListDTO()
			{
				Games = gamesQuery.Select(MapperHelpers.Instance.Map<GameDTO>).ToList(),
				IsMax = isMax,
				TotalCount = totalCount,
				Page = page,
			};
			return list;
		}

		public async Task<GameListDTO> GetGamesByFilterWithPagination(FilterGameDTO filter, int page, CancellationToken cancellationToken)
		{
			var gamesQuery = await UoW.Games.GetAllAsync(cancellationToken);
			gamesQuery = FilterGames(gamesQuery, filter);
			gamesQuery = SortGames(gamesQuery, filter.OrderBy);
			gamesQuery = TakeRange(gamesQuery, page, out int totalCount, out bool isMax);
			var list = new GameListDTO()
			{
				Games = gamesQuery.Select(MapperHelpers.Instance.Map<GameDTO>).ToList(),
				IsMax = isMax,
				TotalCount = totalCount,
				Page = page,
			};
			return list;
		}

		private static IQueryable<Game> TakeRange(IQueryable<Game> gamesQuery, int page, out int totalCount, out bool isMax)
		{
			var gameCount = page * Constants.GAMES_ON_PAGE;
			totalCount = gamesQuery.Count();
			isMax = gameCount >= totalCount;
			Range range = new((page - 1) * Constants.GAMES_ON_PAGE, page * Constants.GAMES_ON_PAGE);
			gamesQuery = gamesQuery.Take(range);
			return gamesQuery;
		}

		private static IQueryable<Game> FilterGames(IQueryable<Game> games, FilterGameDTO filter)
		{
			var filteredGames = games;

			if (filter.GenreIds.Any())
			{
				filteredGames = filteredGames.Where(game =>
					filter.GenreIds.Any(genreId => game.Genres.Any(genre => genre.Id == genreId)));
			}
			if (filter.PlatformIds.Any())
			{
				filteredGames = filteredGames.Where(game =>
					filter.PlatformIds.Any(platformId => game.CopyType != null && game.CopyType.PlatformId == platformId));
			}
			if (filter.RegionIds.Any())
			{
				filteredGames = filteredGames.Where(game =>
					filter.RegionIds.Any(regionId => game.CopyType != null && game.CopyType.AvailableRegions.Any(region => region.Id == regionId)));
			}
			if (filter.TagIds.Any())
			{
				filteredGames = filteredGames.Where(game =>
					filter.TagIds.All(tagId => game.Tags.Any(tag => tagId == tag.Id)));
			}
			if (filter.DeveloperId is not 0)
			{
				filteredGames = filteredGames.Where(game => game.DeveloperId == filter.DeveloperId);
			}
			if (filter.PublisherId is not 0)
			{
				filteredGames = filteredGames.Where(game => game.PublisherId == filter.PublisherId);
			}
			if (filter.IsAvailable)
			{
				filteredGames = filteredGames.Where(game => game.IsAvailable);
			}
			if (filter.IsDiscounted)
			{
				filteredGames = filteredGames.Where(game => game.DiscountPrice != null);
			}
			if (filter.IsHotOffer)
			{
				filteredGames = filteredGames.Where(game => game.IsHotOffer);
			}
			if (filter.PriceFrom is not null)
			{
				filteredGames = filteredGames.Where(game =>
					game.DiscountPrice != null ? game.DiscountPrice >= filter.PriceFrom : game.Price >= filter.PriceFrom);
			}
			if (filter.PriceTo is not null)
			{
				filteredGames = filteredGames.Where(game =>
					game.DiscountPrice != null ? game.DiscountPrice <= filter.PriceTo : game.Price <= filter.PriceTo);
			}
			if (filter.DateFrom.HasValue)
			{
				filteredGames = filteredGames.Where(game => game.Released >= filter.DateFrom);
			}
			if (filter.DateTo.HasValue)
			{
				filteredGames = filteredGames.Where(game => game.Released <= filter.DateTo);
			}
			if (!string.IsNullOrEmpty(filter.SearchQuery))
			{
				filteredGames = filteredGames.Where(game => game.Title.Contains(filter.SearchQuery, StringComparison.OrdinalIgnoreCase));
			}

			return filteredGames;
		}

		private static IQueryable<Game> SortGames(IQueryable<Game> games, OrderBy orderBy)
		{
			return orderBy switch
			{
				OrderBy.DEFAULT => games.OrderBy(game => game.Title).ThenBy(game => game.Title),
				OrderBy.NEWER => games.OrderByDescending(game => game.Released).ThenBy(game => game.Title),
				OrderBy.OLDER => games.OrderBy(game => game.Released).ThenBy(game => game.Title),
				OrderBy.EXPENSIVEST => games.OrderByDescending(game => game.DiscountPrice ?? game.Price).ThenBy(game => game.Title),
				OrderBy.CHEAPEST => games.OrderBy(game => game.DiscountPrice ?? game.Price).ThenBy(game => game.Title),
				OrderBy.POPULARITY => games.OrderByDescending(game => game.SoldCopies).ThenBy(game => game.Title),
				_ => games,
			};
		}

		public async IAsyncEnumerable<GameDTO> GetGames(int[] ids, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in UoW.Games.GetAll(item => ids.Contains(item.Id), cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameDTO>(game);
			}
		}

		public async Task<FilterFormDTO> GetFilterData(CancellationToken cancellationToken)
		{
			var filter = new FilterFormDTO();

			await foreach (var copyType in UoW.CopyTypes.GetAll(cancellationToken))
			{
				filter.CopyTypes.Add(MapperHelpers.Instance.Map<CopyTypeDTO>(copyType));
			}

			await foreach (var platform in UoW.Platforms.GetAll(cancellationToken))
			{
				filter.Platforms.Add(MapperHelpers.Instance.Map<PlatformDTO>(platform));
			}

			await foreach (var genre in UoW.Genres.GetAll(cancellationToken))
			{
				filter.Genres.Add(MapperHelpers.Instance.Map<GenreDTO>(genre));
			}

			await foreach (var genre in UoW.Tags.GetAll(cancellationToken))
			{
				filter.Tags.Add(MapperHelpers.Instance.Map<TagDTO>(genre));
			}

			await foreach (var developer in UoW.Developers.GetAll(cancellationToken))
			{
				filter.Developers.Add(MapperHelpers.Instance.Map<DeveloperDTO>(developer));
			}

			await foreach (var publisher in UoW.Publishers.GetAll(cancellationToken))
			{
				filter.Publishers.Add(MapperHelpers.Instance.Map<PublisherDTO>(publisher));
			}

			await foreach (var region in UoW.Regions.GetAll(cancellationToken))
			{
				filter.AvailableRegions.Add(MapperHelpers.Instance.Map<RegionDTO>(region));
			}

			await foreach (var imageType in UoW.ImageTypes.GetAll(cancellationToken))
			{
				filter.ImageTypes.Add(MapperHelpers.Instance.Map<ImageTypeDTO>(imageType));
			}

			return filter;
		}

		public async Task<GameDTO> AddGame(GameDTO game)
		{
			var newGame = new Game();
			UpdateProperties(game, newGame);
			await ProcessGameGenres(game, newGame);
			await ProcessGameTags(game, newGame);
			await ProcessGameImages(game, newGame);
			await ProcessGameDeveloper(game, newGame);
			await ProcessGamePublisher(game, newGame);
			await ProcessGameCopyType(game, newGame);
			int id = (await UoW.Games.AddAsync(newGame)).Id;
			return MapperHelpers.Instance.Map<GameDTO>(await UoW.Games.GetAsync(id));
		}

		public async Task<GameDTO?> EditGame(GameDTO game)
		{
			var gameFromDB = await UoW.Games.GetAsync(game.Id);
			if (gameFromDB is null) return null;
			UpdateProperties(game, gameFromDB);

			await ProcessGameGenres(game, gameFromDB);
			await ProcessGameTags(game, gameFromDB);
			await ProcessGameImages(game, gameFromDB);
			await ProcessGameDeveloper(game, gameFromDB);
			await ProcessGamePublisher(game, gameFromDB);
			await ProcessGameCopyType(game, gameFromDB);

			return MapperHelpers.Instance.Map<GameDTO>(await UoW.Games.ModifyAsync(gameFromDB));
		}

		private void UpdateProperties(GameDTO game, Game toUpdate)
		{
			toUpdate.Title = game.Title;
			toUpdate.Description = game.Description;
			toUpdate.Price = game.Price;
			toUpdate.IsAvailable = game.IsAvailable;
			toUpdate.IsHotOffer = game.IsHotOffer;
			if (game.DiscountPrice is not null && game.DiscountPrice != toUpdate.DiscountPrice)
			{
				toUpdate.DiscountPrice = game.DiscountPrice;
				_subscriptionService.NotifyDiscount(MapperHelpers.Instance.Map<GameDTO>(toUpdate));
			}
			toUpdate.DiscountPrice = game.DiscountPrice;
			toUpdate.Released = game.Released;
		}

		private async Task ProcessGameGenres(GameDTO game, Game toUpdate)
		{
			toUpdate.Genres ??= new();
			foreach (var item in toUpdate.Genres.ToList())
			{
				toUpdate.Genres.Remove(item);
			}

			foreach (var genre in game.Genres.ToList())
			{
				if (genre.Id is 0)
				{
					toUpdate.Genres.Add(MapperHelpers.Instance.Map<Genre>(genre));
					continue;
				}
				else if (genre.Id is not 0)
				{
					var fromDb = await UoW.Genres.GetAsync(genre.Id);
					if (fromDb is null) continue;
					toUpdate.Genres.Add(fromDb);
				}
			}
		}

		private async Task ProcessGameTags(GameDTO game, Game toUpdate)
		{
			toUpdate.Tags ??= new();
			foreach (var item in toUpdate.Tags.ToList())
			{
				toUpdate.Tags.Remove(item);
			}

			foreach (var tag in game.Tags.ToList())
			{
				if (tag.Id is 0)
				{
					toUpdate.Tags.Add(MapperHelpers.Instance.Map<Tag>(tag));
					continue;
				}
				else if (tag.Id is not 0)
				{
					var fromDb = await UoW.Tags.GetAsync(tag.Id);
					if (fromDb is null) continue;
					toUpdate.Tags.Add(fromDb);
				}
			}
		}

		private async Task ProcessGameImages(GameDTO game, Game toUpdate)
		{
			List<Image> imagesToDelete = toUpdate.Images.Where(image => !game.Images.Any(item => item.Id == image.Id))
				.Select(MapperHelpers.Instance.Map<Image>).ToList();

			toUpdate.Images ??= new();
			foreach (var item in toUpdate.Images.ToList())
			{
				toUpdate.Images.Remove(item);
			}

			foreach (var image in game.Images.ToList())
			{
				if (image.Id is 0)
				{
					var toAdd = MapperHelpers.Instance.Map<Image>(image);
					if (toAdd.TypeId is not 0)
					{
						toAdd.Type = await UoW.ImageTypes.GetAsync(toAdd.TypeId);
					}

					toUpdate.Images.Add(toAdd);
					continue;
				}
				else
				{
					var fromDb = await UoW.Images.GetAsync(image.Id);
					if (fromDb is null) continue;
					toUpdate.Images.Add(fromDb);
				}
			}
			foreach (var image in imagesToDelete.ToList())
			{
				await DeleteImage(image);
			}
		}

		private async Task ProcessGamePublisher(GameDTO game, Game toUpdate)
		{
			if (game.PublisherId is not 0 &&
							!game.PublisherId.Equals(toUpdate.PublisherId))
			{
				if (toUpdate.Id is 0)
				{
					toUpdate.PublisherId = game.PublisherId;
					return;
				}
				toUpdate.Publisher = await UoW.Publishers.GetAsync(game.PublisherId);
				return;
			}
			if (game.PublisherId is 0)
			{
				if (game.Publisher is null) return;

				toUpdate.Publisher = await UoW.Publishers.AddAsync(new()
				{
					Id = 0,
					Name = game.Publisher.Name,
				});
			}
		}

		private async Task ProcessGameDeveloper(GameDTO game, Game toUpdate)
		{
			if (game.DeveloperId is not 0 &&
							!game.DeveloperId.Equals(toUpdate.DeveloperId))
			{
				if (toUpdate.Id is 0)
				{
					toUpdate.DeveloperId = game.DeveloperId;
					return;
				}
				toUpdate.Developer = await UoW.Developers.GetAsync(game.DeveloperId);
				return;
			}
			if (game.DeveloperId is 0)
			{
				if (game.Developer is null) return;

				toUpdate.Developer = await UoW.Developers.AddAsync(new()
				{
					Id = 0,
					Name = game.Developer.Name,
				});
			}
		}

		private async Task ProcessGameCopyType(GameDTO game, Game toUpdate)
		{
			if (game.CopyType is null || game.CopyTypeId is null)
			{
				toUpdate.CopyType = null;
				toUpdate.CopyTypeId = null;
				return;
			}
			if (game.CopyTypeId is not 0 &&
							!game.CopyTypeId.Equals(toUpdate.CopyTypeId))
			{
				if (toUpdate.Id is 0)
				{
					toUpdate.CopyTypeId = game.CopyTypeId;
					return;
				}
				toUpdate.CopyType = await UoW.CopyTypes.GetAsync((int)game.CopyTypeId);
				return;
			}
			if (game.CopyTypeId is 0)
			{
				var newCopyType = new CopyType()
				{
					Id = 0,
					Name = game.CopyType.Name,
					AvailableRegions = new(),
				};
				await ProcessCopyType(game.CopyType, newCopyType);

				toUpdate.CopyType = await UoW.CopyTypes.AddAsync(newCopyType);
			}
		}

		private async Task ProcessCopyType(CopyTypeDTO copy, CopyType toUpdate)
		{
			if (copy.PlatformId is not 0 &&
				!copy.PlatformId.Equals(toUpdate.Id))
			{
				toUpdate.Platform = await UoW.Platforms.GetAsync(copy.PlatformId);
			}
			if (copy.PlatformId is 0 && copy.Platform is not null)
			{
				toUpdate.Platform = new Platform()
				{
					Id = 0,
					Name = copy.Platform.Name
				};
				toUpdate.PlatformId = 0;
			}
			toUpdate.AvailableRegions ??= new();

			foreach (var region in copy.AvailableRegions.ToList())
			{
				if (region.Id is 0)
				{
					toUpdate.AvailableRegions.Add(MapperHelpers.Instance.Map<Region>(region));
					continue;
				}
				else
				{
					var fromDb = await UoW.Regions.GetAsync(region.Id);
					if (fromDb is null) continue;
					toUpdate.AvailableRegions.Add(fromDb);
				}
			}
		}

		public async Task<bool> DeleteGame(int id)
		{
			if (id == 0) return false;
			var game = await UoW.Games.GetAsync(id);
			if (game == null) return false;

			foreach (var image in game.Images.ToList())
			{
				await DeleteImage(image);
			}
			foreach (var copy in game.Copies.ToList())
			{
				await UoW.Copies.DeleteAsync(copy);
			}
			try
			{
				await UoW.Games.DeleteAsync(game);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteGameWithSerializedData(int id, string serialized)
		{
			var game = await UoW.Games.GetAsync(id);
			if (game == null) return false;

			DeletedDataManager.EnsureDeletedDataDirectoryExists();
			string newFile = Path.Combine(DeletedDataManager.DeletedDataDirectory, game.Title + ".txt");
			using StreamWriter sw = File.AppendText(newFile);
			await sw.WriteLineAsync(serialized);
			await sw.DisposeAsync();

			foreach (var image in game.Images.ToList())
			{
				await DeleteImage(image);
			}
			foreach (var copy in game.Copies.ToList())
			{
				await UoW.Copies.DeleteAsync(copy);
			}

			await UoW.Games.DeleteAsync(game);
			return true;
		}

		private async Task DeleteImage(Image image)
		{
			await UoW.Images.DeleteAsync(image);
			if (image.ActualPath is not null && File.Exists(image.ActualPath))
			{
				File.Delete(image.ActualPath);
			}
		}

		public async IAsyncEnumerable<GenreDTO> GetGenres([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var genre in UoW.Genres.GetAll(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GenreDTO>(genre);
			}
		}

		public async Task<ImageDTO?> BindImageToGame(int gameId, ImageFormDTO model)
		{
			var game = await UoW.Games.GetAsync(gameId);
			if (game is null)
			{
				return null;
			}
			var image = new Image();

			if (model.TypeId == 0)
			{
				image.Type = new ImageType()
				{
					Name = model.Type
				};
			}
			else
			{
				image.TypeId = model.TypeId;
			}

			game.Images.Add(await UoW.Images.AddAsync(image));

			string relativePath = "/files/game images/" + game.Title + '/';
			string rootPath = _appEnvironment.WebRootPath;
			Directory.CreateDirectory(rootPath + relativePath);
			string actualFileName = +image.Id + model.Image.FileName;
			string actualPath = rootPath + relativePath + actualFileName;

			using (var fileStream = new FileStream(actualPath, FileMode.Create))
			{
				await model.Image.CopyToAsync(fileStream);
			}
			var serverPath = _server.Features.Get<IServerAddressesFeature>()?.Addresses.First() + relativePath;

			image.Path = serverPath + actualFileName;
			image.ActualPath = actualPath;
			await UoW.Games.ModifyAsync(game);
			return MapperHelpers.Instance.Map<ImageDTO>(image);
		}
	}
}