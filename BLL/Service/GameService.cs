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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnitsOfWork.Interfaces;

namespace BLL.Service
{
	public class GameService : IGameService
	{

		private readonly IUnitOfWork UoW;
		private readonly ILogger<GameService> _logger;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly ISubscriptionService _subscriptionService;
		private readonly IServer _server;

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
			if (filter.GenreIds.Any() || filter.PlatformIds.Any() || filter.RegionIds.Any())
				await foreach (var game in UoW.Games.GetAll().WithCancellation(cancellationToken))
				{
					if (filter.GenreIds.Any() &&
						!filter.GenreIds.Any(item => game.Genres.Any(genre => genre.Id == item)))
					{
						continue;
					}
					if (filter.PlatformIds.Any() &&
						!filter.PlatformIds.Any(game.CopyType.Platform.Id.Equals))
					{
						continue;
					}

					if (filter.RegionIds.Any() &&
						!filter.RegionIds.Any(regionId => game.CopyType.AvailableRegions.Any(item => item.Id == regionId)))
					{
						continue;
					}
					filteredGames.Add(MapperHelper.Instance.Map<GameDTO>(game));
				}
			else
			{
				filteredGames.AddRange(GetGames().ToBlockingEnumerable(cancellationToken));
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
			var newGame = new Game();
			UpdateProperties(game, newGame);
			await ProcessGameGenres(game, newGame);
			await ProcessGameImages(game, newGame);
			await ProcessGameDeveloper(game, newGame);
			await ProcessGamePublisher(game, newGame);
			await ProcessGameCopyType(game, newGame);

			return MapperHelper.Instance.Map<GameDTO>(await UoW.Games.AddAsync(newGame));
		}

		public async Task<GameDTO> EditGame(int id, GameDTO game)
		{
			var gameFromDB = await UoW.Games.GetAsync(id);
			UpdateProperties(game, gameFromDB);

			await ProcessGameGenres(game, gameFromDB);
			await ProcessGameImages(game, gameFromDB);
			await ProcessGameDeveloper(game, gameFromDB);
			await ProcessGamePublisher(game, gameFromDB);
			await ProcessGameCopyType(game, gameFromDB);

			return MapperHelper.Instance.Map<GameDTO>(await UoW.Games.ModifyAsync(id, gameFromDB));
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
				_subscriptionService.NotifyDiscount(MapperHelper.Instance.Map<GameDTO>(toUpdate));
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
					toUpdate.Genres.Add(MapperHelper.Instance.Map<Genre>(genre));
					continue;
				}
				else if (genre.Id is not 0)
				{
					toUpdate.Genres.Add(await UoW.Genres.GetAsync(genre.Id));
					continue;
				}
			}
		}

		private async Task ProcessGameImages(GameDTO game, Game toUpdate)
		{

			toUpdate.Images ??= new();
			foreach (var item in toUpdate.Images.ToList())
			{
				toUpdate.Images.Remove(item);
			}

			foreach (var image in game.Images.ToList())
			{
				if (image.Id is 0)
				{
					toUpdate.Images.Add(MapperHelper.Instance.Map<Image>(image));
					continue;
				}
				else
				{
					toUpdate.Images.Add(await UoW.Images.GetAsync(image.Id));
				}
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
				toUpdate.Developer = await UoW.Developers.AddAsync(new()
				{
					Id = 0,
					Name = game.Developer.Name,
				});
			}
		}

		private async Task ProcessGameCopyType(GameDTO game, Game toUpdate)
		{
			if (game.CopyTypeId is not 0 &&
							!game.CopyTypeId.Equals(toUpdate.CopyTypeId))
			{
				if (toUpdate.Id is 0)
				{
					toUpdate.CopyTypeId = game.CopyTypeId;
					return;
				}
				toUpdate.CopyType = await UoW.CopyTypes.GetAsync(game.CopyTypeId);
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
			if (copy.PlatformId is 0)
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
					toUpdate.AvailableRegions.Add(MapperHelper.Instance.Map<Region>(region));
					continue;
				}
				else
				{
					toUpdate.AvailableRegions.Add(await UoW.Regions.GetAsync(region.Id));
				}
			}
		}

		public async Task<bool> DeleteGame(int id, string serialized)
		{
			var game = await UoW.Games.GetAsync(id);
			if (game == null) return false;

			string contentRootPath = _appEnvironment.ContentRootPath;
			string deletedDataDirectory = contentRootPath[..contentRootPath.LastIndexOf("\\")] + "\\Deleted data";
			string newFile = deletedDataDirectory + "\\" + game.Title + ".txt";
			using StreamWriter sw = File.AppendText(newFile);
			await sw.WriteLineAsync(serialized);
			await sw.DisposeAsync();
			sw.Close();

			if (game.Images is not null)
				foreach (var image in game.Images.ToList())
				{
					await DeleteImage(image);
				}
			if (game.Copies is not null)
				foreach (var copy in game.Copies.ToList())
				{
					await UoW.Copies.DeleteAsync(copy.Id);
				}
			await UoW.Games.DeleteAsync(game);
			return true;
		}

		private async Task DeleteImage(Image? image)
		{
			await UoW.Images.DeleteAsync(image.Id);
			if (image.ActualPath is not null && File.Exists(image.ActualPath))
			{
				File.Delete(image.ActualPath);
			}
		}

		public async IAsyncEnumerable<GenreDTO> GetGenres()
		{
			await foreach (var genre in UoW.Genres.GetAll())
			{
				yield return MapperHelper.Instance.Map<GenreDTO>(genre);
			}
		}

		public async Task<ImageDTO?> BindImageToGame(int gameId, ImageFormDTO model)
		{
			var game = await UoW.Games.GetAsync(gameId);
			var image = new Image()
			{
				Name = model.FileName,
			};
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
			var serverPath = _server.Features.Get<IServerAddressesFeature>().Addresses.First() + relativePath;
			

			image.Path = serverPath + actualFileName;
			image.ActualPath = actualPath;
			await UoW.Games.ModifyAsync(game.Id, game);
			return MapperHelper.Instance.Map<ImageDTO>(image);
		}
	}
}