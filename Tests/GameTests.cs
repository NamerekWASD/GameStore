using API.Models.Games;
using API.Tools;
using BLL.DTO;
using BLL.Service.Games;
using BLL.Service.Mails;
using BLL.Tools;
using DAL.Context;
using DAL.Managers;
using DAL.UoW;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace Tests
{
	public class Tests
	{
		private List<GameDTO> _games;
		private Mock<GameService> mockGameService;
		private Mock<UnitOfWork> mockUnitOfWork;

		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<GameContext>();
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MockGamesdb;Trusted_Connection=True;");
			var mockContext = new GameContext(optionsBuilder.Options);
			mockUnitOfWork = new Mock<UnitOfWork>(mockContext);
			var mockAppEnvironment = new Mock<IWebHostEnvironment>();
			var mockLogger = new Mock<ILogger<GameService>>();
			var mockSubscriptionService = new Mock<ISubscriptionService>();
			var mockServer = new Mock<IServer>();
			// ����������� ����� GameService � �������� �����������
			mockGameService = new Mock<GameService>(mockUnitOfWork.Object,
										  mockAppEnvironment.Object,
										  mockLogger.Object,
										  mockSubscriptionService.Object,
										  mockServer.Object);
			_games = new List<GameDTO>();
			InitGames();
		}

		private void InitGames()
		{
			TagDTO openWorld = new()
			{
				Name = "³������� ���"
			};

			TagDTO firstPersonShooter = new()
			{
				Name = "����� �� ����� �����"
			};

			TagDTO actionAdventure = new()
			{
				Name = "����-�������"
			};

			TagDTO multiplayer = new()
			{
				Name = "��������������������"
			};

			TagDTO rolePlaying = new()
			{
				Name = "������� ���"
			};

			TagDTO scienceFiction = new()
			{
				Name = "������� ����������"
			};

			TagDTO fantasy = new()
			{
				Name = "������"
			};

			TagDTO tactical = new()
			{
				Name = "�������"
			};

			DeveloperDTO fromSoftware = new()
			{
				Name = "FromSoftware",
			};

			PublisherDTO BandaiNamco = new()
			{
				Name = "FromSoftware, Bandai Namco Entertainment",
			};

			PlatformDTO steam = new()
			{
				Name = "Steam"
			};

			GenreDTO action = new()
			{
				Name = "�������"
			};

			var EastEurope = new List<RegionDTO>()
			{
				new ()
				{
					Name = "������ ������"
				}
			};
			CopyTypeDTO key = new()
			{
				Name = "����",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			ImageTypeDTO poster = new()
			{
				Name = Constants.POSTER_IMAGE,
			};

			GenreDTO RPG = new()
			{
				Name = "RPG"
			};
			CopyDTO copy1 = new()
			{
				Data = "1243dfs-safdbcx321-sdfvcx321-gdfxvc"
			};
			CopyDTO copy2 = new()
			{
				Data = "1243dfs-saf234x321-sdfvcx321-gdfxvc"
			};
			CopyDTO copy3 = new()
			{
				Data = "1243dfs-sax321-sdfvcx321-gdfxvc"
			};
			GameDTO elden = new()
			{
				Title = "Elden Ring",
				Images = new()
				{
					new()
					{
						Type = poster,
						Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg"
					},
					new()
					{
						Type = poster,
						Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg"
					}
				},
				Description = "Elden Ring � �� ������� ���-���� 2022 ����, ���������� FromSoftware " +
				"� ������ Bandai Namco Entertainment. Elden Ring ������������ ����� ����������� �� " +
				"������ �����, �� ������ ����� �������� ������������� �������� �����. س��� �������� " +
				"�������� ����� ����������, �������������� ������� Torrent �� �������� ��� ����������, " +
				"� ������ ��������� ��������� ����� �����������, ��� ������ ������� ��������. ��� ����������� " +
				"������� ������ ���� �� �������� ������������, ������� � �������� ���, ��������� ��������� ���������. " +
				"����������-��������� ������, ����������� �� ������ ��������� ����, ���������� ������� ����������� ��� " +
				"�������� �� ��������� �������������� ������ �� ������ ����, � ����� ���� �� �������, �� ���������� ������ " +
				"������������. � Elden Ring � ������������������� ������-���, � ��� ������ ������ ������������ ���� �� ������ " +
				"��� ���������� �� ��� �� ��������.",
				Released = new DateTime(2022, 2, 25),
				Genres = new() { action, RPG },
				Tags = new() { openWorld, actionAdventure, rolePlaying, fantasy },
				Developer = fromSoftware,
				Publisher = BandaiNamco,
				Price = 44.9M,
				CopyType = key,
				DiscountPrice = 34.9M,
				IsAvailable = true,
				IsHotOffer = true,
				Copies = new() { copy1, copy2, copy3 },
				SoldCopies = 43534,
			};

			PublisherDTO Bethesda = new()
			{
				Name = "Bethesda Softworks"
			};
			DeveloperDTO IdSoftware = new()
			{
				Name = "id Software"
			};

			GenreDTO FPS = new()
			{
				Name = "FPS"
			};

			CopyDTO copy4 = new()
			{
				Data = "1243dfs-safdbcx321-sdfvcx321-gdfxvc"
			};
			CopyDTO copy5 = new()
			{
				Data = "1243dfs-saf234x321-sdfvcx321-gdfxvc"
			};
			CopyDTO copy6 = new()
			{
				Data = "1243dfs-sax321-sdfvcx321-gdfxvc"
			};

			GameDTO DoomEternal = new()
			{
				Title = "Doom Eternal",
				Images = new()
				{
					new()
					{
						Type = poster,
						Path = "https://upload.wikimedia.org/wikipedia/ru/8/8c/%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0_Doom_Eternal.jpg"
					}
				},
				Description = "Doom Eternal � �� ����� �� ����� �����, ����������� ��������� id Software " +
				"�� ������������ Bethesda Softworks. ����������� ��� Doom (2016) � ����� ��� ��� Doom, �������� 20 ������� 2020 ����. " +
				"����� ������������ ����� ������ ��� ���� ���� ��� 2016 ����. ������ ����� �������� " +
				"��� Doom Slayer, ���� ������ ��� ��������� ���������� ���� ������ � ������ ����� ����������� " +
				"������� ���� �������� �������.",
				Released = new DateTime(2020, 3, 20),
				Genres = new() { FPS },
				Tags = new() { firstPersonShooter, actionAdventure, scienceFiction },
				Developer = IdSoftware,
				Publisher = Bethesda,
				Price = 29.9M,
				IsAvailable = true,
				IsHotOffer = false,
				CopyType = key,
				Copies = new() { copy4, copy5, copy6 },
				SoldCopies = 1346,
			};

			PublisherDTO Ubisoft = new()
			{
				Name = "Ubisoft",
			};

			DeveloperDTO UbisoftMontreal = new()
			{
				Name = "Ubisoft Montreal",
			};

			GenreDTO tacticalShooter = new()
			{
				Name = "��������� �����",
			};

			CopyTypeDTO account = new()
			{
				Name = "������",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			CopyDTO copy7 = new()
			{
				Data = "Loggin: GroMyka\nPassword: 534624"
			};
			CopyDTO copy8 = new()
			{
				Data = "Loggin: 4etvertak\nPassword: dsfsd432"
			};
			CopyDTO copy9 = new()
			{
				Data = "Loggin: Ne4to\nPassword: 21345"
			};

			GameDTO TCRSS = new()
			{
				Title = "Tom Clancy's Rainbow Six Siege",
				Images = new()
				{
					new()
					{
						Type = poster,
						Path = "https://cdn1.epicgames.com/offer/carnation/Carousel_BoxArt_1200x1600_1200x1600-6888b9d57181d8fcfb3472a7f70ecc49"
					}
				},
				Description = "Tom Clancy's Rainbow Six Siege � �������� ��������� �������-�����, " +
				"���������� Ubisoft Montreal � ������ Ubisoft. � �� ������ ����� ����������� " +
				"���������� ������������� ���������� �� �������� �� ��������. ����� ������� ���� " +
				"�� ���� �������� ��� ���������� ��� ���������� � ����� ������� ���, ����� �� ��������� " +
				"���������, ������������ ����� �� �������� ��� �ᒺ���� � �����. � ���� ���� ��������, ��� ������ " +
				"���� �������� ������-���, �� ����������� �����������, �� ����� ���������� ��������. ֳ �� ����� " +
				"������ �������, ������������ �� �����������, �� ��������� ��������, ��� ���������� �� �� ��������� ������� " +
				"� ������� �������, ������������� ������, ��� ������� ������� ����",
				Released = new DateTime(2015, 12, 1),
				Genres = new() { FPS, tacticalShooter },
				Tags = new() { multiplayer, firstPersonShooter, tactical },
				Developer = UbisoftMontreal,
				Publisher = Ubisoft,
				Price = 29.9M,
				IsAvailable = true,
				IsHotOffer = false,
				CopyType = account,
				Copies = new() { copy7, copy8, copy9 },
				SoldCopies = 8543,
			};
			_games = new List<GameDTO>() { elden, DoomEternal, TCRSS };
		}

		[TearDown]
		public void TearDown()
		{
			mockUnitOfWork.Object.DeleteDB();
		}

		[Test]
		public void AddingGame()
		{
			var eldenRing = _games.Find(item => item.Title == "Elden Ring") ?? _games[0];

			GameDTO gameFromDb = mockGameService.Object.AddGame(eldenRing).Result;

			Assert.That(gameFromDb, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(gameFromDb.Title, Is.EqualTo(eldenRing.Title));
				Assert.That(gameFromDb.Description, Is.EqualTo(eldenRing.Description));
				Assert.That(gameFromDb.Released, Is.EqualTo(eldenRing.Released));
				Assert.That(gameFromDb.Publisher, Is.Not.Null);
				Assert.That(gameFromDb.Publisher.Name, Is.EqualTo(eldenRing.Publisher.Name));
				Assert.That(gameFromDb.Genres, Has.Count.EqualTo(eldenRing.Genres.Count));
				Assert.That(gameFromDb.Images, Has.Count.EqualTo(eldenRing.Images.Count));
				Assert.That(gameFromDb.Price, Is.EqualTo(eldenRing.Price));
				Assert.That(gameFromDb.DiscountPrice, Is.Not.Null);
				Assert.That(gameFromDb.DiscountPrice, Is.EqualTo(eldenRing.DiscountPrice));
				Assert.That(gameFromDb.GetFirstPoster.Id, Is.Not.EqualTo(0));
			});
		}

		[Test]
		public void EditingGame()
		{
			var eldenRing = _games.Find(item => item.Title == "Elden Ring") ?? _games[0];

			GameDTO gameFromDb = mockGameService.Object.AddGame(eldenRing).Result;

			string newTitle = "New Title";
			string NewDescritpion = "New Description";
			RegionDTO region = new() { Name = "Asia" };
			GenreDTO genre = new() { Name = "Some" };
			DateTime released = DateTime.Now;
			ImageTypeDTO imageType = new() { Name = "screenShot" };
			ImageDTO image = new() { Type = imageType };
			TagDTO tag = new() { Name = "Some" };

			gameFromDb.Title = newTitle;
			gameFromDb.Description = NewDescritpion;
			gameFromDb.Price = 1.99M;
			gameFromDb.DiscountPrice = null;

			var genreToRemove = gameFromDb.Genres.First();
			gameFromDb.Genres.Remove(genreToRemove);
			gameFromDb.Genres.Add(genre);

			var tagToRemove = gameFromDb.Tags.First();
			gameFromDb.Tags.Remove(tagToRemove);
			gameFromDb.Tags.Add(tag);

			var replacedCopyType = gameFromDb.CopyType;
			gameFromDb.CopyTypeId = 0;
			gameFromDb.CopyType = new CopyTypeDTO()
			{
				Name = "KEY",
				AvailableRegions = new() { region },
				PlatformId = replacedCopyType.PlatformId,
			};

			gameFromDb.Released = released;

			var imageToDelete = gameFromDb.Images.First();
			gameFromDb.Images.Add(image);
			gameFromDb = mockGameService.Object.EditGame(gameFromDb).Result;

			Assert.That(gameFromDb, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(gameFromDb.Title, Is.EqualTo(newTitle));
				Assert.That(gameFromDb.Description, Is.EqualTo(NewDescritpion));
				Assert.That(gameFromDb.Released, Is.EqualTo(released));
				Assert.That(gameFromDb.Publisher, Is.Not.Null);
				Assert.That(gameFromDb.Developer, Is.Not.Null);
				Assert.That(gameFromDb.Genres.Any(x => x.Id.Equals(genreToRemove.Id)), Is.False);
				Assert.That(gameFromDb.Genres.Any(item => item.Name.Equals(genre.Name)), Is.True);
				Assert.That(gameFromDb.Tags.Any(x => x.Id.Equals(tagToRemove.Id)), Is.False);
				Assert.That(gameFromDb.Tags.Any(item => item.Name.Equals(tag.Name)), Is.True);
				Assert.That(gameFromDb.CopyType.AvailableRegions.Any(item => item.Name.Equals(region.Name)), Is.True);
				Assert.That(gameFromDb.CopyType.Id.Equals(replacedCopyType.Id), Is.False);
				Assert.That(gameFromDb.CopyType.PlatformId.Equals(replacedCopyType.Id), Is.True);
				Assert.That(gameFromDb.CopyType.AvailableRegions.Any(item => item.Equals(replacedCopyType.AvailableRegions.Any(region => region.Id == item.Id))), Is.False);
				Assert.That(gameFromDb.Price, Is.EqualTo(1.99M));
				Assert.That(gameFromDb.DiscountPrice, Is.Null);
				Assert.That(gameFromDb.Images.Any(item => item.Type.Name.Equals(image.Type.Name)), Is.True);
			});
		}

		[Test]
		public void DeleteGame()
		{
			var eldenRing = _games.Find(item => item.Title == "Elden Ring") ?? _games[0];

			GameDTO gameFromDb = mockGameService.Object.AddGame(eldenRing).Result;
			Assert.Multiple(() =>
			{
				Assert.That(mockGameService.Object.GetGame(gameFromDb.Id).Result.Id is not 0, Is.True);
				Assert.That(mockGameService.Object.DeleteGame(gameFromDb.Id).Result, Is.True);
			});
		}

		[Test]
		public void DeleteGameWithSerializedData()
		{
			var eldenRing = _games.Find(item => item.Title == "Elden Ring") ?? _games[0];

			var gameFromDb = API.Tools.MapperHelpers.Instance.Map<GameModel>(mockGameService.Object.AddGame(eldenRing).Result);

			JsonSerializerOptions options = new()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			string serialized = JsonSerializer.Serialize(gameFromDb, options);

			Assert.Multiple(() =>
			{
				Assert.That(mockGameService.Object.GetGame(gameFromDb.Id).Result.Id is not 0, Is.True);
				Assert.That(mockGameService.Object.DeleteGameWithSerializedData(gameFromDb.Id, serialized).Result, Is.True);
			});
			serialized = string.Empty;
			string file = Path.Combine(DeletedDataManager.DeletedDataDirectory, gameFromDb.Title + ".txt");
			using StreamReader sw = File.OpenText(file);
			serialized = sw.ReadToEnd();
			var data = JsonSerializer.Deserialize<GameModel>(serialized);
			Assert.That(data, Is.Not.Null);
			Assert.That(data.Id, Is.EqualTo(gameFromDb.Id));
		}
	}
}