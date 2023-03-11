using API.Models.Games;
using BLL.DTO.Games;
using BLL.Interface;
using BLL.Service;
using DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading;
using UnitsOfWork.Interfaces;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize("Manager")]
	public class GameController : ControllerBase
	{
		private readonly IGameService _gameService;

		private readonly ILogger<GameController> _logger;

		public GameController(GameContext context, ILogger<GameController> logger)
		{
			_gameService = new GameService(context);
			_logger = logger;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult GetGameList(CancellationToken cancellationToken)
		{
			return Ok(GetGameLightsAsync(cancellationToken));
		}
		[HttpGet("detailed")]
		[AllowAnonymous]
		public IActionResult GetGameDetailedList(CancellationToken cancellationToken)
		{
			return Ok(GetGameDetailedAsync(cancellationToken));
		}
		[HttpPost("list")]
		[AllowAnonymous]
		public IActionResult GetGameListByIds([FromBody] int[] ids, CancellationToken cancellationToken)
		{
			return Ok(GetGameLightsByIdsAsync(ids, cancellationToken));
		}
		[HttpGet("genres")]
		[AllowAnonymous]
		public IActionResult GetGenres(CancellationToken cancellationToken)
		{
			return Ok(GetGenresAsync(cancellationToken));
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			return Ok(MapperHelpers.Instance.Map<GameDetails>(await _gameService.GetGame(id).WaitAsync(cancellationToken)));
		}
		[HttpGet("genre/{id}")]
		[AllowAnonymous]
		public IActionResult GetByGenreId(int id, CancellationToken cancellationToken)
		{
			return Ok(GetGamesByGenre(id, cancellationToken));
		}

		[HttpPost]
		public async Task<IActionResult> CreateGame([FromBody] GameModel game, CancellationToken cancellationToken)
		{
			return Ok(await _gameService.AddGame(MapperHelpers.Instance.Map<GameDTO>(game)).WaitAsync(cancellationToken));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditGame(int id, [FromBody] GameModel game, CancellationToken cancellationToken)
		{
			return Ok(await _gameService.EditGame(id, MapperHelpers.Instance.Map<GameDTO>(game)).WaitAsync(cancellationToken));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGame(int id, CancellationToken cancellationToken)
		{
			return Ok(await _gameService.DeleteGame(id).WaitAsync(cancellationToken));
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
	}
}