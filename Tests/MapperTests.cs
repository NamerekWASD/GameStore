using API.Models.Games;
using BLL.DTO;
using DAL.Entity;

namespace Tests
{
	public class MapperTests
	{
		[Test]
		public void GameEntityToGameDto_PreservesCoreFieldsAndCollectionCounts()
		{
			var game = new Game
			{
				Title = "Elden Ring",
				Price = 44.9M,
				DiscountPrice = 34.9M,
				Released = new DateTime(2022, 2, 25),
				Genres = new() { new Genre { Name = "RPG" }, new Genre { Name = "Action" } },
				Tags = new() { new Tag { Name = "Open World" } },
				Images = new() { new Image { Path = "a.jpg" }, new Image { Path = "b.jpg" } },
			};

			var dto = BLL.Tools.MapperHelpers.Instance.Map<GameDTO>(game);

			Assert.Multiple(() =>
			{
				Assert.That(dto.Title, Is.EqualTo("Elden Ring"));
				Assert.That(dto.Price, Is.EqualTo(44.9M));
				Assert.That(dto.DiscountPrice, Is.EqualTo(34.9M));
				Assert.That(dto.Released, Is.EqualTo(new DateTime(2022, 2, 25)));
				Assert.That(dto.Genres, Has.Count.EqualTo(2));
				Assert.That(dto.Tags, Has.Count.EqualTo(1));
				Assert.That(dto.Images, Has.Count.EqualTo(2));
			});
		}

		[Test]
		public void GameDtoToGameModelAndBack_RoundTripPreservesValues()
		{
			var dto = new GameDTO
			{
				Title = "Doom Eternal",
				Description = "Fast-paced FPS",
				Price = 29.9M,
				DiscountPrice = null,
				Released = new DateTime(2020, 3, 20),
				IsAvailable = true,
				IsHotOffer = false,
				SoldCopies = 1346,
			};

			var model = API.Tools.MapperHelpers.Instance.Map<GameModel>(dto);
			var roundTripped = API.Tools.MapperHelpers.Instance.Map<GameDTO>(model);

			Assert.Multiple(() =>
			{
				Assert.That(model.Title, Is.EqualTo(dto.Title));
				Assert.That(model.Description, Is.EqualTo(dto.Description));
				Assert.That(model.Price, Is.EqualTo(dto.Price));
				Assert.That(model.Released, Is.EqualTo(dto.Released));

				Assert.That(roundTripped.Title, Is.EqualTo(dto.Title));
				Assert.That(roundTripped.Description, Is.EqualTo(dto.Description));
				Assert.That(roundTripped.Price, Is.EqualTo(dto.Price));
				Assert.That(roundTripped.DiscountPrice, Is.EqualTo(dto.DiscountPrice));
				Assert.That(roundTripped.Released, Is.EqualTo(dto.Released));
				Assert.That(roundTripped.IsAvailable, Is.EqualTo(dto.IsAvailable));
				Assert.That(roundTripped.SoldCopies, Is.EqualTo(dto.SoldCopies));
			});
		}

		[Test]
		public void GameDtoToGameModel_NullDeveloperPublisherCopyType_MapToNullWithoutThrowing()
		{
			var dto = new GameDTO
			{
				Title = "No Metadata Game",
				Developer = null,
				Publisher = null,
				CopyType = null,
			};

			GameModel model = default!;
			Assert.DoesNotThrow(() => model = API.Tools.MapperHelpers.Instance.Map<GameModel>(dto));
			Assert.Multiple(() =>
			{
				Assert.That(model.Developer, Is.Null);
				Assert.That(model.Publisher, Is.Null);
				Assert.That(model.CopyType, Is.Null);
			});
		}
	}
}
