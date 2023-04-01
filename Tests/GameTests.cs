

using BLL.DTO.Copies;
using BLL.DTO.Developers;
using BLL.DTO.Games;
using BLL.DTO.GameType;
using BLL.DTO.Gernres;
using BLL.DTO.Images;
using BLL.DTO.Platforms;
using BLL.DTO.Publishers;
using BLL.DTO.Regions;
using BLL.DTO.Tags;
using BLL.Service.Games;
using BLL.Service.Mails;
using BLL.Tools;
using DAL.Context;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using DAL.UoW;
using API.Models.Games;
using API.Tools;
using System.Text.Json;
using DAL.Entity.Games;
using DAL.Managers;

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
			// Ініціалізуємо сервіс GameService з макетами залежностей
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
				Name = "Відкритий світ"
			};

			TagDTO firstPersonShooter = new()
			{
				Name = "Шутер від першої особи"
			};

			TagDTO actionAdventure = new()
			{
				Name = "Екшн-пригода"
			};

			TagDTO multiplayer = new()
			{
				Name = "Багатокористувацький"
			};

			TagDTO rolePlaying = new()
			{
				Name = "Рольова гра"
			};

			TagDTO scienceFiction = new()
			{
				Name = "Наукова фантастика"
			};

			TagDTO fantasy = new()
			{
				Name = "Фентезі"
			};

			TagDTO tactical = new()
			{
				Name = "Тактика"
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
				Name = "Бойовик"
			};

			var EastEurope = new List<RegionDTO>()
			{
				new ()
				{
					Name = "Східна Європа"
				}
			};
			CopyTypeDTO key = new()
			{
				Name = "Ключ",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			ImageTypeDTO portrait = new()
			{
				Name = Constants.PORTRAIT_IMAGE,
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
						Type = portrait,
						Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg"
					},
					new()
					{
						Type = portrait,
						Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg"
					}
				},
				Description = "Elden Ring — це рольова гра-екшн 2022 року, розроблена FromSoftware " +
				"і видана Bandai Namco Entertainment. Elden Ring представлено через перспективу від " +
				"третьої особи, де гравці вільно блукають інтерактивним відкритим світом. Шість основних " +
				"територій можна пересувати, використовуючи скакуна Torrent як основний вид транспорту, " +
				"а лінійні приховані підземелля можна досліджувати, щоб знайти корисні предмети. Бій полегшується " +
				"кількома типами зброї та магічними заклинаннями, включно з непрямим боєм, увімкнутим механікою скритності. " +
				"Контрольно-пропускні пункти, розташовані по всьому відкритому світу, дозволяють гравцеві покращувати свої " +
				"атрибути за допомогою внутрішньоігрової валюти під назвою Руни, а також діють як локації, що дозволяють швидко " +
				"подорожувати. У Elden Ring є багатокористувацька онлайн-гра, у якій гравці можуть приєднуватися один до одного " +
				"для кооперації та бою між гравцями.",
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
						Type = portrait,
						Path = "https://upload.wikimedia.org/wikipedia/ru/8/8c/%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0_Doom_Eternal.jpg"
					}
				},
				Description = "Doom Eternal — це шутер від першої особи, розроблений компанією id Software " +
				"та опублікований Bethesda Softworks. Продовження гри Doom (2016) і сьома гра серії Doom, випущена 20 березня 2020 року. " +
				"Сюжет розгортається через деякий час після подій гри 2016 року. Історія знову розповідає " +
				"про Doom Slayer, який виконує місію припинити поглинання Землі Пеклом і зірвати плани інопланетян " +
				"Майкрсів щодо знищення людства.",
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
				Name = "Тактичний шутер",
			};

			CopyTypeDTO account = new()
			{
				Name = "Акаунт",
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
						Type = portrait,
						Path = "https://cdn1.epicgames.com/offer/carnation/Carousel_BoxArt_1200x1600_1200x1600-6888b9d57181d8fcfb3472a7f70ecc49"
					}
				},
				Description = "Tom Clancy's Rainbow Six Siege — тактична онлайнова відеогра-шутер, " +
				"розроблена Ubisoft Montreal і видана Ubisoft. У грі велика увага приділяється " +
				"руйнуванню навколишнього середовища та співпраці між гравцями. Кожен гравець бере " +
				"на себе контроль над нападником або захисником у різних режимах гри, таких як порятунок " +
				"заручника, знешкодження бомби та контроль над об’єктом у кімнаті. У назві немає кампанії, але містить " +
				"серію коротких офлайн-місій, які називаються «ситуаціями», які можна виконувати поодинці. Ці місії мають " +
				"вільний наратив, зосереджений на новобранцях, які проходять навчання, щоб підготувати їх до майбутніх зіткнень " +
				"з «Білими масками», терористичною групою, яка загрожує безпеці світу",
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
				Assert.That(gameFromDb.GetFirstPortrait.Id, Is.Not.EqualTo(0));
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

			var gameFromDb = MapperHelpers.Instance.Map<GameModel>(mockGameService.Object.AddGame(eldenRing).Result);

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