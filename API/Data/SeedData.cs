using API.Models.Games;
using DAL.Context;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using Microsoft.EntityFrameworkCore;

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
				Name = "Action"
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
				Name = "key",
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
				ImageURL = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg",
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
				Copies = new List<Copy>() { copy1, copy2, copy3 },
				Sold = 6243,
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
				ImageURL = "https://upload.wikimedia.org/wikipedia/ru/8/8c/%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0_Doom_Eternal.jpg",
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
				CopyType = key,
				Copies = new List<Copy>() { copy4, copy5, copy6 },
				Sold = 1346,
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
				Name = "Account",
				Platform = steam,
				AvailableRegions = EastEurope,
			};

			Copy copy7 = new()
			{
				Data = "1243dfs-safdbcx321-sdfvcx321-gdfxvc"
			};
			Copy copy8 = new()
			{
				Data = "1243dfs-saf234x321-sdfvcx321-gdfxvc"
			};
			Copy copy9 = new()
			{
				Data = "1243dfs-sax321-sdfvcx321-gdfxvc"
			};

			Game TCRSS = new()
			{
				Title = "Tom Clancy's Rainbow Six Siege",
				ImageURL = "https://cdn1.epicgames.com/offer/carnation/Carousel_BoxArt_1200x1600_1200x1600-6888b9d57181d8fcfb3472a7f70ecc49",
				Description= "Tom Clancy's Rainbow Six Siege — тактична онлайнова відеогра-шутер, " +
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
				CopyType = account,
				Copies = new List<Copy>() { copy7, copy8, copy9 },
				Sold = 8543,
			};
			context.Games.AddRange(elden, DoomEternal, TCRSS);
			context.SaveChanges();
		}
	}
}
