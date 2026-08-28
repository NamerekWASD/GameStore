using BLL.DTO;
using BLL.Service.Games;
using BLL.Service.Mails;
using DAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
	public class GameServiceTests
	{
		private GameService _service;

		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<GameContext>();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var context = new GameContext(optionsBuilder.Options);

			_service = new GameService(context,
				new Mock<IWebHostEnvironment>().Object,
				new Mock<ILogger<GameService>>().Object,
				new Mock<ISubscriptionService>().Object,
				new Mock<IServer>().Object);
		}

		private static GameDTO MakeGame(string title, decimal? price = 10M, decimal? discountPrice = null, DateTime? released = null)
		{
			return new GameDTO
			{
				Title = title,
				Price = price,
				DiscountPrice = discountPrice,
				Released = released ?? new DateTime(2020, 1, 1),
				IsAvailable = true,
			};
		}

		[Test]
		public async Task AddGame_CanBeFoundById()
		{
			var added = await _service.AddGame(MakeGame("Elden Ring"));

			var found = await _service.GetGame(added.Id);

			Assert.That(found.Id, Is.EqualTo(added.Id));
			Assert.That(found.Title, Is.EqualTo("Elden Ring"));
		}

		[Test]
		public async Task DeleteGame_RemovesItFromLookup()
		{
			var added = await _service.AddGame(MakeGame("Doom Eternal"));

			var deleted = await _service.DeleteGame(added.Id);
			var found = await _service.GetGame(added.Id);

			Assert.That(deleted, Is.True);
			Assert.That(found, Is.Null);
		}

		[Test]
		public async Task FilterByPriceRange_ExcludesGamesOutsideRange()
		{
			await _service.AddGame(MakeGame("Cheap Game", price: 5M));
			await _service.AddGame(MakeGame("Mid Game", price: 25M));
			await _service.AddGame(MakeGame("Expensive Game", price: 60M));

			var filter = new FilterGameDTO { PriceFrom = 10M, PriceTo = 30M };
			var result = await _service.GetGamesByFilterWithPagination(filter, 1, CancellationToken.None);

			Assert.That(result.Games.Select(g => g.Title), Is.EquivalentTo(new[] { "Mid Game" }));
		}

		[Test]
		public async Task FilterByIsDiscounted_OnlyReturnsGamesWithDiscountPrice()
		{
			await _service.AddGame(MakeGame("On Sale", discountPrice: 5M));
			await _service.AddGame(MakeGame("Full Price"));

			var filter = new FilterGameDTO { IsDiscounted = true };
			var result = await _service.GetGamesByFilterWithPagination(filter, 1, CancellationToken.None);

			Assert.That(result.Games.Select(g => g.Title), Is.EquivalentTo(new[] { "On Sale" }));
		}

		[Test]
		public async Task FilterBySearchQuery_FindsGameByPartialTitleCaseInsensitive()
		{
			await _service.AddGame(MakeGame("The Witcher 3"));
			await _service.AddGame(MakeGame("Cyberpunk 2077"));

			var filter = new FilterGameDTO { SearchQuery = "witcher" };
			var result = await _service.GetGamesByFilterWithPagination(filter, 1, CancellationToken.None);

			Assert.That(result.Games.Select(g => g.Title), Is.EquivalentTo(new[] { "The Witcher 3" }));
		}

		[Test]
		public async Task OrderByCheapest_SortsAscendingByEffectivePrice()
		{
			await _service.AddGame(MakeGame("B Game", price: 30M));
			await _service.AddGame(MakeGame("A Game", price: 10M));
			await _service.AddGame(MakeGame("C Game", price: 20M));

			var filter = new FilterGameDTO { OrderBy = OrderBy.CHEAPEST };
			var result = await _service.GetGamesByFilterWithPagination(filter, 1, CancellationToken.None);

			Assert.That(result.Games.Select(g => g.Title), Is.EqualTo(new[] { "A Game", "C Game", "B Game" }));
		}
	}
}
