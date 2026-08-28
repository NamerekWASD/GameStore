using BLL.DTO;
using BLL.Tools;

namespace Tests
{
	public class GameDtoTests
	{
		[Test]
		public void GetFirstPoster_ReturnsPosterImage_NotFirstImage()
		{
			var screenshotType = new ImageTypeDTO { Id = 1, Name = "SCREENSHOT" };
			var posterType = new ImageTypeDTO { Id = 2, Name = Constants.POSTER_IMAGE };
			var game = new GameDTO
			{
				Images = new()
				{
					new ImageDTO { Id = 1, Type = screenshotType, Path = "screenshot.jpg" },
					new ImageDTO { Id = 2, Type = posterType, Path = "poster.jpg" },
				}
			};

			var poster = game.GetFirstPoster;

			Assert.That(poster.Path, Is.EqualTo("poster.jpg"));
		}

		[Test]
		public void GetFirstPoster_EmptyImages_ReturnsEmptyImageDtoWithoutThrowing()
		{
			var game = new GameDTO { Images = new() };

			ImageDTO poster = default!;
			Assert.DoesNotThrow(() => poster = game.GetFirstPoster);
			Assert.That(poster, Is.Not.Null);
			Assert.That(poster.Id, Is.EqualTo(0));
		}

		[Test]
		public void GetFirstPoster_ImagesWithNullType_AreIgnored()
		{
			var game = new GameDTO
			{
				Images = new()
				{
					new ImageDTO { Id = 1, Type = null, Path = "no-type.jpg" },
				}
			};

			var poster = game.GetFirstPoster;

			Assert.That(poster.Id, Is.EqualTo(0));
		}

		[Test]
		public void GetRegions_CopyTypeNull_ReturnsEmptyArray()
		{
			var game = new GameDTO { CopyType = null };

			Assert.That(game.GetRegions, Is.Empty);
		}

		[Test]
		public void GetRegions_ReturnsAllRegionNamesFromCopyType()
		{
			var game = new GameDTO
			{
				CopyType = new CopyTypeDTO
				{
					AvailableRegions = new()
					{
						new RegionDTO { Name = "Europe" },
						new RegionDTO { Name = "Asia" },
					}
				}
			};

			Assert.That(game.GetRegions, Is.EquivalentTo(new[] { "Europe", "Asia" }));
		}

		[Test]
		public void GetGenres_ReturnsNameForEveryGenre()
		{
			var game = new GameDTO
			{
				Genres = new()
				{
					new GenreDTO { Name = "RPG" },
					new GenreDTO { Name = "Action" },
					new GenreDTO { Name = "Adventure" },
				}
			};

			Assert.That(game.GetGenres, Has.Length.EqualTo(3));
			Assert.That(game.GetGenres, Is.EquivalentTo(new[] { "RPG", "Action", "Adventure" }));
		}

		[Test]
		public void GetTags_ReturnsNameForEveryTag()
		{
			var game = new GameDTO
			{
				Tags = new()
				{
					new TagDTO { Name = "Open World" },
					new TagDTO { Name = "Multiplayer" },
				}
			};

			Assert.That(game.GetTags, Has.Length.EqualTo(2));
			Assert.That(game.GetTags, Is.EquivalentTo(new[] { "Open World", "Multiplayer" }));
		}
	}
}
