using API.Models.Games;
using BLL.Tools;
using DAL.Context;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API.Data
{
	public class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using var context = new GameContext(serviceProvider.GetRequiredService<DbContextOptions<GameContext>>());
			// Look for any movies.
			if (context.Games.Any())
			{
				return;
			}
			Developer fromSoftware = new()
			{
				Name = "FromSoftware",
			};
			Publisher BandaiNamco = new()
			{
				Name = "FromSoftware, Bandai Namco Entertainment",
			};

			Platform steam = new()
			{
				Name = "Steam"
			};

			Genre action = new()
			{
				Name = "Бойовик"
			};

			var EastEurope = new List<Region>()
			{
				new Region()
				{
					Name = "Східна Європа"
				}
			};
			CopyType key = new()
			{
				Name = "Ключ",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			Genre RPG = new()
			{
				Name = "RPG"
			};
			Copy copy1 = new()
			{
				Data = "1243dfs-safdbcx321-sdfvcx321-gdfxvc"
			};
			Copy copy2 = new()
			{
				Data = "1243dfs-saf234x321-sdfvcx321-gdfxvc"
			};
			Copy copy3 = new()
			{
				Data = "1243dfs-sax321-sdfvcx321-gdfxvc"
			};
			Game elden = new()
			{
				Title = "Elden Ring",
				Images = new() { new() { Name = Constants.PORTRAIT_IMAGE, Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg" } },
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
				Genres = new List<Genre>() { action, RPG },
				Developer = fromSoftware,
				Publisher = BandaiNamco,
				Price = 44.9M,
				CopyType = key,
				DiscountPrice = 34.9M,
				IsAvailable = true,
				IsHotOffer = true,
				Copies = new List<Copy>() { copy1, copy2, copy3 },
				SoldCopies = 43534,
			};

			Publisher Bethesda = new()
			{
				Name = "Bethesda Softworks"
			};
			Developer IdSoftware = new()
			{
				Name = "id Software"
			};

			Genre FPS = new()
			{
				Name = "FPS"
			};

			Copy copy4 = new()
			{
				Data = "1243dfs-safdbcx321-sdfvcx321-gdfxvc"
			};
			Copy copy5 = new()
			{
				Data = "1243dfs-saf234x321-sdfvcx321-gdfxvc"
			};
			Copy copy6 = new()
			{
				Data = "1243dfs-sax321-sdfvcx321-gdfxvc"
			};

			Game DoomEternal = new()
			{
				Title = "Doom Eternal",
				Images = new() { new() { Name = Constants.PORTRAIT_IMAGE, Path = "https://upload.wikimedia.org/wikipedia/ru/8/8c/%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0_Doom_Eternal.jpg" } },
				Description = "Doom Eternal — це шутер від першої особи, розроблений компанією id Software " +
				"та опублікований Bethesda Softworks. Продовження гри Doom (2016) і сьома гра серії Doom, випущена 20 березня 2020 року. " +
				"Сюжет розгортається через деякий час після подій гри 2016 року. Історія знову розповідає " +
				"про Doom Slayer, який виконує місію припинити поглинання Землі Пеклом і зірвати плани інопланетян " +
				"Майкрсів щодо знищення людства.",
				Released = new DateTime(2020, 3, 20),
				Genres = new List<Genre>() { FPS },
				Developer = IdSoftware,
				Publisher = Bethesda,
				Price = 29.9M,
				IsAvailable = true,
				IsHotOffer = false,
				CopyType = key,
				Copies = new List<Copy>() { copy4, copy5, copy6 },
				SoldCopies = 1346,
			};

			Publisher Ubisoft = new()
			{
				Name = "Ubisoft",
			};

			Developer UbisoftMontreal = new()
			{
				Name = "Ubisoft Montreal",
			};

			Genre tacticalShooter = new()
			{
				Name = "Тактичний шутер",
			};

			CopyType account = new()
			{
				Name = "Акаунт",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			Copy copy7 = new()
			{
				Data = "Loggin: GroMyka\nPassword: 534624"
			};
			Copy copy8 = new()
			{
				Data = "Loggin: 4etvertak\nPassword: dsfsd432"
			};
			Copy copy9 = new()
			{
				Data = "Loggin: Ne4to\nPassword: 21345"
			};

			Game TCRSS = new()
			{
				Title = "Tom Clancy's Rainbow Six Siege",
				Images = new() { new() { Name = Constants.PORTRAIT_IMAGE, Path = "https://cdn1.epicgames.com/offer/carnation/Carousel_BoxArt_1200x1600_1200x1600-6888b9d57181d8fcfb3472a7f70ecc49" } },
				Description = "Tom Clancy's Rainbow Six Siege — тактична онлайнова відеогра-шутер, " +
				"розроблена Ubisoft Montreal і видана Ubisoft. У грі велика увага приділяється " +
				"руйнуванню навколишнього середовища та співпраці між гравцями. Кожен гравець бере " +
				"на себе контроль над нападником або захисником у різних режимах гри, таких як порятунок " +
				"заручника, знешкодження бомби та контроль над об’єктом у кімнаті. У назві немає кампанії, але містить " +
				"серію коротких офлайн-місій, які називаються «ситуаціями», які можна виконувати поодинці. Ці місії мають " +
				"вільний наратив, зосереджений на новобранцях, які проходять навчання, щоб підготувати їх до майбутніх зіткнень " +
				"з «Білими масками», терористичною групою, яка загрожує безпеці світу",
				Released = new DateTime(2015, 12, 1),
				Genres = new List<Genre>() { FPS, tacticalShooter },
				Developer = UbisoftMontreal,
				Publisher = Ubisoft,
				Price = 29.9M,
				IsAvailable = true,
				IsHotOffer = false,
				CopyType = account,
				Copies = new List<Copy>() { copy7, copy8, copy9 },
				SoldCopies = 8543,
			};

			Publisher rockstarGames = new()
			{
				Name = "Rockstar Games",
			};

			Developer rockStar = new()
			{
				Name = "Rockstar Games",
			};

			Region Global = new()
			{
				Name = "Глобальний"
			};

			Platform epicGames = new()
			{
				Name = "Epic Games"
			};

			Genre adventure = new()
			{
				Name = "Пригодницький",
			};

			Genre TPS = new()
			{
				Name = "TPS",
			};

			CopyType accountEpicGames = new()
			{
				Name = "Акаунт",
				AvailableRegions = new() { Global },
				Platform = epicGames,
			};

			Copy copy10 = new()
			{
				Data = "Loggin: Voskres$Nebes\nPassword: 5432cxvsd"
			};

			Copy copy11 = new()
			{
				Data = "Loggin: Sumail$uk\nPassword: 25fdszvc"
			};


			Game GTA5 = new()
			{
				Title = "Grand Theft Auto V",
				Images = new() { new() { Name = Constants.PORTRAIT_IMAGE, Path = "https://upload.wikimedia.org/wikipedia/ru/c/c8/GTAV_Official_Cover_Art.jpg" } },
				Description = "Grand Theft Auto V (скор. GTA V) — це пригодницька відеогра 2013 року, розроблена " +
				"Rockstar North та видана компанією Rockstar Games. Події відбуваються у вигаданому штаті Сан-Андреас, " +
				"що заснований на південній Каліфорнії. Сюжетна історія слідує за трьома злочинцями, " +
				"які перебувають під тиском урядового відомства та авторитетних кримінальних фігур. Дизайн відкритого світу " +
				"дозволяє гравцям вільно переміщуватися по місцевості Сан-Андреаса та вигаданому місту Лос-Сантос, на базі Лос-Анджелеса." +
				"\r\n\r\nГра відбувається з виглядом від третьої, або першої особи, а її світом подорожують пішки або на транспорті. " +
				"Гравці керують трьома головними героями протягом однокористувацької гри та перемикаються між ними, як під час місій, " +
				"так і поза ними. Історія зосереджена на послідовних крадіжках, а багато місій передбачають стрілянину та керування транспортом. " +
				"Система «розшуку» регулює агресію правоохоронців на злочини які вчинив гравець. Grand Theft Auto Online — онлайн мультиплеєр, " +
				"дозволяє до 30 гравцям брати участь у різноманітних кооперативних та конкурентних режимах гри.",
				Released = new DateTime(2015, 4, 14),
				Genres = new List<Genre>() { action, adventure, TPS },
				Developer = rockStar,
				Publisher = rockstarGames,
				Price = 24.9M,
				DiscountPrice = 19.9M,
				IsAvailable = true,
				IsHotOffer = true,
				CopyType = accountEpicGames,
				Copies = new List<Copy>() { copy10, copy11 },
				SoldCopies = 34254,
			};

			Publisher cdProject = new()
			{
				Name = "CD Projekt",
			};

			Developer cdProjectRed = new()
			{
				Name = "CD Projekt RED",
			};

			Copy copy13 = new()
			{
				Data = "Loggin: MiKro4eLik\nPassword: 34tgdfdsv"
			};

			Copy copy14 = new()
			{
				Data = "Loggin: Bogdan33\nPassword: 12dzsc345"
			};

			Copy copy15 = new()
			{
				Data = "Loggin: Bogdan33\nPassword: 12dzsc345"
			};

			Copy copy16 = new()
			{
				Data = "Loggin: Bogdan33\nPassword: 12dzsc345"
			};


			Copy copy17 = new()
			{
				Data = "Loggin: Bogdan33\nPassword: 12dzsc345"
			};

			Copy copy18 = new()
			{
				Data = "Loggin: Bogdan33\nPassword: 12dzsc345"
			};

			Game cyberpunk2077 = new()
			{
				Title = "Cyberpunk 2077",
				Images = new() { new() { Name = Constants.PORTRAIT_IMAGE, Path = "https://m.media-amazon.com/images/M/MV5BMGU4ODVkZDYtYjNiNS00YmFiLWFmOWQtODM2NDRjZDAxNzliXkEyXkFqcGdeQXVyMTI0MzA4NTgw._V1_FMjpg_UX1000_.jpg" } },
				Description = "«Cyberpunk 2077» (укр. «Кіберпанк 2077») — відеогра в жанрі action RPG в стилі кіберпанку, розроблена польською студією CD Projekt RED на ігровому рушії REDengine 4. " +
				"Гра є адаптацією настільної рольової гри Cyberpunk 2020, " +
				"її події відбуваються на п'ятдесят сім років пізніше у вигаданому місті " +
				"Найт-Сіті, Каліфорнія, надаючи відкритий світ з шістьма різними регіонами. " +
				"Гравці беруть на себе роль найманця Ві, стать і зовнішність якого налаштовуються " +
				"за бажанням. На одному з завдань Ві стає мимовільним свідком убивства глави корпорації, " +
				"що створила чип для перенесення свідомості. Вину за вбивство покладають на Ві та застрелюють. " +
				"Але завдяки чипу Ві виживає і береться відновити справедливість, разом з тим шукаючи спосіб позбутися " +
				"несподіваних побічних ефектів від носіння чипа.",
				Released = new DateTime(2020, 12, 10),
				Genres = new List<Genre>() { action, adventure, RPG },
				Developer = cdProjectRed,
				Publisher = cdProject,
				Price = 59.9M,
				 DiscountPrice = 25M ,
				IsAvailable = true,
				IsHotOffer = true,
				CopyType = accountEpicGames,
				Copies = new List<Copy>() { copy13, copy14, copy15, copy16, copy17, copy18, },
				SoldCopies = 23463,
			};

			Developer naughtyDog = new()
			{
				Name = "CD Projekt",
			};

			Publisher SIEntertaiment = new()
			{
				Name = "Sony Interactive Entertainment",
			};

			Copy copy19 = new()
			{
				Data = "Loggin: Mikro4elicks\nPassword: 432efzcsrt",
			};

			Platform PS5 = new()
			{
				Name = "PlayStation 5"
			};

			CopyType accountPSStore = new()
			{
				Name = "Акаунт",
				AvailableRegions = new() { Global },
				Platform = PS5,
			};

			Game theLastOfUs1 = new()
			{
				Title = "The Last of Us Part I",
				Images = new() {
					new()
					{
						Name = Constants.PORTRAIT_IMAGE,
						Path = "https://upload.wikimedia.org/wikipedia/ru/2/21/The_Last_of_Us_Part_I_-_%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0.png"
					}
				},
				Description = "The Last of Us Part I — відеогра в жанрі пригодницького бойовика, " +
				"розроблена Naughty Dog і видана Sony Interactive Entertainment для PlayStation 5 " +
				"у вересні 2022 року. Порт для Microsoft Windows буде випущено в березні 2023 року. " +
				"Вона є римейком The Last of Us (2013) та має перероблений ігровий процес, включно з покращеними " +
				"бойовою механікою та дослідженням середовища, а також розширені спеціальні можливості. Сюжетна історія " +
				"оповідає про Джоела[en], який має супроводжувати дівчину-підлітка Еллі через постапокаліптичні Сполучені Штати " +
				"та захищатися від канібалістичних істот, заражених штамом вірусу грибка кордицепс, що мутував. Гра включає доповнення " +
				"The Last of Us: Left Behind[en], яке оповідає про Еллі та її кращу подругу Райлі. Багатокористувацький режим з оригінальної " +
				"гри не було додано до римейку.",
				Released = new DateTime(2022, 9, 2),
				Genres = new List<Genre>() { action, adventure },
				Developer = naughtyDog,
				Publisher = SIEntertaiment,
				IsAvailable = false,
				IsHotOffer = true,
				Price = 59.9M,
				CopyType = accountPSStore,
				Copies = new List<Copy>() { copy19 },
				SoldCopies = 5481,
			};

			context.Games.AddRange(elden, DoomEternal, TCRSS, GTA5, cyberpunk2077, theLastOfUs1);
			context.SaveChanges();
		}
	}
}
