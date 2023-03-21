using API.Models.Filters;
using API.Models.Games;
using API.Models.Images;
using BLL.DTO.Filters;
using BLL.DTO.Games;
using BLL.DTO.Images;
using BLL.Interface;
using BLL.Service;
using BLL.Tools;
using DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using UnitsOfWork.Interfaces;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GameController : ControllerBase
	{
		private readonly IGameService _gameService;
		private readonly IMailService _mailService;
		private readonly ILogger<GameController> _logger;

		public GameController(
			IGameService gameService,
			IMailService mailService,
			ILogger<GameController> logger)
		{
			_gameService = gameService;
			_mailService = mailService;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult GetGameList(CancellationToken cancellationToken)
		{
			return Ok(GetGameLightsAsync(cancellationToken));
		}

		[HttpGet("detailed")]
		public IActionResult GetGameDetailedList(CancellationToken cancellationToken)
		{
			return Ok(GetGameDetailedAsync(cancellationToken));
		}

		[HttpPost("list")]
		public IActionResult GetGameListByIds([FromBody] int[] ids, CancellationToken cancellationToken)
		{
			return Ok(GetGameLightsByIdsAsync(ids, cancellationToken));
		}
		[HttpGet("genres")]
		public IActionResult GetGenres(CancellationToken cancellationToken)
		{
			return Ok(GetGenresAsync(cancellationToken));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			var game = MapperHelpers.Instance.Map<GameDetails>(await _gameService.GetGame(id).WaitAsync(cancellationToken));
			if (game == null) return BadRequest();
			return Ok(game);
		}

		[HttpGet("genre/{id}")]
		public IActionResult GetByGenreId(int id, CancellationToken cancellationToken)
		{
			return Ok(GetGamesByGenre(id, cancellationToken));
		}

		[HttpGet("search")]
		public IActionResult SearchByQuery([FromQuery(Name = "query")] string query, CancellationToken cancellationToken)
		{
			return Ok(GetGamesByQuery(query, cancellationToken).ToBlockingEnumerable(cancellationToken).AsQueryable().Take(5));
		}

		[HttpGet("filter")]
		public async Task<IActionResult> FormFilterData(CancellationToken cancellationToken)
		{
			var filterData = await _gameService.GetFilterData(cancellationToken);
			return Ok(MapperHelpers.Instance.Map<FilterFormDataModel>(filterData));
		}

		[HttpPost("filter")]
		public async Task<IActionResult> SearchByFilter([FromBody] FilterGameModel filter, CancellationToken cancellationToken)
		{
			var games = await _gameService.GetGamesByFilter(MapperHelpers.Instance.Map<FilterGameDTO>(filter), cancellationToken);
			return Ok(games.Select(MapperHelpers.Instance.Map<GameDetails>));
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
			var game = MapperHelpers.Instance.Map<GameModel>(await _gameService.GetGame(id).WaitAsync(cancellationToken));
			if (game == null) return NotFound("Гру не знайдено!");
			return Ok(game);
		}

		[HttpPost("create")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> CreateGame([FromBody] GameModel game)
		{
			await _gameService.AddGame(MapperHelpers.Instance.Map<GameDTO>(game));
			return Ok();
		}

		[HttpPut("edit/{id}")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> EditGame(int id, [FromBody] GameModel game)
		{
			await _gameService.EditGame(id, MapperHelpers.Instance.Map<GameDTO>(game));
			return Ok();	
		}

		[HttpDelete("delete/{id}")]
		[Authorize(Constants.MANAGER)]
		public async Task<IActionResult> DeleteGame(int id, CancellationToken cancellationToken)
		{
			JsonSerializerOptions options = new()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			string serrialized = JsonSerializer.Serialize(MapperHelpers.Instance.Map<GameModel>(await _gameService.GetGame(id)), options);
			var result = await _gameService.DeleteGame(id, serrialized);
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
			var imageDTO = await _gameService.BindImageToGame(id, MapperHelpers.Instance.Map<ImageFormDTO>(model));
			if (imageDTO is null)
			{
				return BadRequest("Сталась помилка...");
			}
			return Ok(MapperHelpers.Instance.Map<ImageModel>(imageDTO));
		}
		private async IAsyncEnumerable<GameLightModel> GetGameLightsAsync([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGames().WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameLightModel>(game);
			}
		}
		private async IAsyncEnumerable<GameDetails> GetGameDetailedAsync([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGames().WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameDetails>(game);
			}
		}
		private async IAsyncEnumerable<GameLightModel> GetGameLightsByIdsAsync(int[] ids, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGames(ids).WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameLightModel>(game);
			}
		}

		private async IAsyncEnumerable<GenreModel> GetGenresAsync([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var genre in _gameService.GetGenres().WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GenreModel>(genre);
			}
		}

		private async IAsyncEnumerable<GameDetails> GetGamesByGenre(int genreId, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGamesByGenre(genreId).WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameDetails>(game);
			}
		}

		private async IAsyncEnumerable<GameLightModel> GetGamesByQuery(string query, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			await foreach (var game in _gameService.GetGamesByQuery(query, cancellationToken).WithCancellation(cancellationToken))
			{
				yield return MapperHelpers.Instance.Map<GameLightModel>(game);
			}
		}
	}
}