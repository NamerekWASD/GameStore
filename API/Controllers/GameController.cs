using API.Models.Filters;
using API.Models;
using BLL.DTO;
using BLL.Service.Games;
using BLL.Service.Genres;
using BLL.Service.Mails;
using BLL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninject.Infrastructure.Language;
using System.Runtime.CompilerServices;
using System.Text.Json;
using API.Models.Lists;
using API.Models.Games;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GameController : ControllerBase
	{
		private readonly IGameService _gameService;
		private readonly IMailService _mailService;
		private readonly IGenreService _genreService;
#pragma warning disable IDE0052 // Remove unread private members
		private readonly ILogger<GameController> _logger;
#pragma warning restore IDE0052 // Remove unread private members

		public GameController(
			IGameService gameService,
			IMailService mailService,
			IGenreService genreService,
			ILogger<GameController> logger)
		{
			_gameService = gameService;
			_mailService = mailService;
			_genreService = genreService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetGameList(CancellationToken cancellationToken,
			[FromQuery(Name = "page")] int page = 1)
		{
			var games = await _gameService.GetGamesWithPagination(page, cancellationToken);
			return base.Ok(Tools.MapperHelpers.Instance.Map<GameListModel>(games));
		}

		[HttpPost("filter")]
		public async Task<IActionResult> SearchByFilter([FromBody] FilterGameModel? filter,
			CancellationToken cancellationToken,
			[FromQuery(Name = "page")] int page = 1)
		{
			if (filter == null)
			{
				var games = await _gameService.GetGamesWithPagination(page, cancellationToken);
				return base.Ok(Tools.MapperHelpers.Instance.Map<GameListModel>(games));
			}
			var gameListModel = await _gameService.GetGamesByFilterWithPagination(Tools.MapperHelpers.Instance.Map<FilterGameDTO>(filter), page, cancellationToken);
			return base.Ok(Tools.MapperHelpers.Instance.Map<GameListModel>(gameListModel));
		}

		[HttpPost("list")]
		public IActionResult GetGameListByIds([FromBody] int[] ids, CancellationToken cancellationToken)
		{
			return Ok(GetGameLightsByIdsAsync(ids, cancellationToken));
		}

		[HttpGet("genres")]
		public IActionResult GetGenres(CancellationToken cancellationToken)
		{
			return base.Ok(_genreService.GetGenres(cancellationToken)
				.ToBlockingEnumerable(cancellationToken: cancellationToken)
				.Select(Tools.MapperHelpers.Instance.Map<GenreModel>));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			var game = Tools.MapperHelpers.Instance.Map<GameDetails>(await _gameService.GetGame(id).WaitAsync(cancellationToken));
			if (game == null) return BadRequest();
			return Ok(game);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> FormFilterData(CancellationToken cancellationToken)
		{
			var filterData = await _gameService.GetFilterData(cancellationToken);
			return base.Ok(Tools.MapperHelpers.Instance.Map<FilterFormDataModel>(filterData));
		}

		[HttpPost("subscribe/{id}")]
		public async Task<IActionResult> SubscribeOnGame(int id, [FromBody] string userEmail)
		{
			return Ok(await _mailService.MakeSubscription(id, userEmail));
		}

		[HttpGet("model/{id}")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> GetModelById(int id, CancellationToken cancellationToken)
		{
			if (id == 0) return NotFound();
			var game = Tools.MapperHelpers.Instance.Map<GameModel>(await _gameService.GetGame(id).WaitAsync(cancellationToken));
			if (game == null) return NotFound("Гру не знайдено!");
			return Ok(game);
		}

		[HttpPost("create")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> CreateGame([FromBody] GameModel game)
		{
			var edited = await _gameService.AddGame(Tools.MapperHelpers.Instance.Map<GameDTO>(game));
			if (edited == null) return NotFound();
			return Ok();
		}

		[HttpPut("edit")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> EditGame([FromBody] GameModel game)
		{
			var edited = await _gameService.EditGame(Tools.MapperHelpers.Instance.Map<GameDTO>(game));
			if (edited == null) return NotFound();
			return Ok();
		}

		[HttpDelete("delete/{id}")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> DeleteGame(int id)
		{
			JsonSerializerOptions options = new()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			string serrialized = JsonSerializer.Serialize(Tools.MapperHelpers.Instance.Map<GameModel>(await _gameService.GetGame(id)), options);
			var result = await _gameService.DeleteGameWithSerializedData(id, serrialized);
			if (result)
				return Ok();
			else
				return BadRequest("Сталась помилка...");
		}

		[HttpPost("upload-image/{id}")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> UploadFile(int id, [FromForm] ImageFormModel model)
		{
			if (model is null || model.Image is null)
			{
				BadRequest("Прикріпіть файл");
			}
			var imageDTO = await _gameService.BindImageToGame(id, Tools.MapperHelpers.Instance.Map<ImageFormDTO>(model));
			if (imageDTO is null)
			{
				return BadRequest("Сталась помилка...");
			}
			return base.Ok(Tools.MapperHelpers.Instance.Map<ImageModel>(imageDTO));
		}

		private async IAsyncEnumerable<GameLightModel> GetGameLightsByIdsAsync(int[] ids, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGames(ids, cancellationToken).WithCancellation(cancellationToken))
			{
				yield return Tools.MapperHelpers.Instance.Map<GameLightModel>(game);
			}
		}
	}
}