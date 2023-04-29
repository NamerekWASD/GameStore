using BLL.Tools;
using DAL.Context;
using DAL.Entity;
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

			Tag openWorld = new()
			{
				Name = "Відкритий світ"
			};

			Tag firstPersonShooter = new()
			{
				Name = "Шутер від першої особи"
			};

			Tag stealth = new()
			{
				Name = "Стелс"
			};

			Tag actionAdventure = new()
			{
				Name = "Екшн-пригода"
			};

			Tag postApocalyptic = new()
			{
				Name = "Післяапокаліптика"
			};

			Tag multiplayer = new()
			{
				Name = "Багатокористувацький"
			};

			Tag rolePlaying = new()
			{
				Name = "Рольова гра"
			};

			Tag openWorldSandbox = new()
			{
				Name = "Відкритий світ / Пісочниця"
			};

			Tag scienceFiction = new()
			{
				Name = "Наукова фантастика"
			};

			Tag fantasy = new()
			{
				Name = "Фентезі"
			};

			Tag crime = new()
			{
				Name = "Кримінальний"
			};

			Tag racing = new()
			{
				Name = "Гонки"
			};

			Tag survival = new()
			{
				Name = "Виживання"
			};

			Tag horror = new()
			{
				Name = "Жахи"
			};

			Tag puzzle = new()
			{
				Name = "Головоломки"
			};
			Tag tactical = new()
			{
				Name = "Тактика"
			};

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

			ImageType poster = new()
			{
				Name = Constants.PORTRAIT_IMAGE,
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
				Images = new() { new() { Type = poster, Path = "https://upload.wikimedia.org/wikipedia/ru/7/7c/Elden_Ring_-_cover.jpg" } },
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
				Tags = new() { openWorld, actionAdventure, rolePlaying, fantasy },
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
				Images = new() { new() { Type = poster, Path = "https://upload.wikimedia.org/wikipedia/ru/8/8c/%D0%9E%D0%B1%D0%BB%D0%BE%D0%B6%D0%BA%D0%B0_Doom_Eternal.jpg" } },
				Description = "Doom Eternal — це шутер від першої особи, розроблений компанією id Software " +
				"та опублікований Bethesda Softworks. Продовження гри Doom (2016) і сьома гра серії Doom, випущена 20 березня 2020 року. " +
				"Сюжет розгортається через деякий час після подій гри 2016 року. Історія знову розповідає " +
				"про Doom Slayer, який виконує місію припинити поглинання Землі Пеклом і зірвати плани інопланетян " +
				"Майкрсів щодо знищення людства.",
				Released = new DateTime(2020, 3, 20),
				Genres = new List<Genre>() { FPS },
				Tags = new() { firstPersonShooter, actionAdventure, scienceFiction },
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
				Images = new() { new() { Type = poster, Path = "https://cdn1.epicgames.com/offer/carnation/Carousel_BoxArt_1200x1600_1200x1600-6888b9d57181d8fcfb3472a7f70ecc49" } },
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
				Tags = new() { multiplayer, firstPersonShooter, tactical },
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
				Images = new() { new() { Type = poster, Path = "https://upload.wikimedia.org/wikipedia/ru/c/c8/GTAV_Official_Cover_Art.jpg" } },
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
				Tags = new() { openWorld, crime, racing, multiplayer },
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
				Images = new() { new() { Type = poster, Path = "https://m.media-amazon.com/images/M/MV5BMGU4ODVkZDYtYjNiNS00YmFiLWFmOWQtODM2NDRjZDAxNzliXkEyXkFqcGdeQXVyMTI0MzA4NTgw._V1_FMjpg_UX1000_.jpg" } },
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
				Tags = new() { openWorld, rolePlaying, scienceFiction },
				Developer = cdProjectRed,
				Publisher = cdProject,
				Price = 59.9M,
				DiscountPrice = 25M,
				IsAvailable = true,
				IsHotOffer = true,
				CopyType = accountEpicGames,
				Copies = new List<Copy>() { copy13, copy14, copy15, copy16, copy17, copy18, },
				SoldCopies = 23463,
			};

			Developer naughtyDog = new()
			{
				Name = "Naughty Dog",
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
						Type = poster,
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
				Tags = new() { actionAdventure, postApocalyptic, horror, survival, puzzle },
				Developer = naughtyDog,
				Publisher = SIEntertaiment,
				IsAvailable = false,
				IsHotOffer = true,
				Price = 59.9M,
				CopyType = accountPSStore,
				Copies = new List<Copy>() { copy19 },
				SoldCopies = 5481,
			};

			Tag sciFi = new()
			{
				Name = "наукова фантастика"
			};
			Tag femaleProtagonist = new()
			{
				Name = "Жінка-протагоніст"
			}; Tag thirdPerson = new()
			{
				Name = "Від третьої особи"
			};

			Copy copy20 = new()
			{
				Data = "Login: ZestkiYZadrot\nPassword: 1234asdf"
			};
			Platform PS4 = new()
			{
				Name = "PlayStation 4"
			};

			Region Asia = new()
			{
				Name = "Азія"
			};

			CopyType PS4Account = new()
			{
				Name = "Акаунт",
				Platform = PS4,
				AvailableRegions = new() { Asia, EastEurope.First() },
			};

			Developer guerrillaGames = new()
			{
				Name = "Guerrilla Games"
			};

			Game HorizonZeroDawn = new()
			{
				Title = "Horizon Zero Dawn",
				Images = new() { new() { Type = poster, Path = "https://static.posters.cz/image/750/%D0%9F%D0%BB%D0%B0%D0%BA%D0%B0%D1%82%D0%B8/horizon-zero-dawn-key-art-i34856.jpg" } },
				Description = "Horizon Zero Dawn — відеогра в жанрі action RPG, розроблена студією Guerrilla Games " +
					"та видана Sony Interactive Entertainment для PlayStation 4 у 2017 році, а для PC у 2020 році. Гра відбувається " +
					"у відкритому світі, де гравець бере на себе роль Алой, юної мисливця із племені Нора. Сюжет розповідає про " +
					"походження Алой, яка живе в світі, де роботи панують, а людство зійшло з доріг. Її задача досліджувати світ, " +
					"боротися з роботами та знаходити відповіді на складні питання.",
				Released = new DateTime(2017, 2, 28),
				Genres = new List<Genre>() { action, RPG },
				Tags = new() { openWorld, postApocalyptic, sciFi, femaleProtagonist, thirdPerson },
				Developer = guerrillaGames,
				Publisher = SIEntertaiment,
				IsAvailable = true,
				IsHotOffer = true,
				Price = 19.99M,
				CopyType = PS4Account,
				Copies = new List<Copy>() { copy20 },
				SoldCopies = 14238,
			};

			Tag medieval = new()
			{
				Name = "Cередньовічний"
			};
			Tag mature = new()
			{
				Name = "Зрілий"
			};
			Tag violence = new()
			{
				Name = "Насильство"
			};
			Copy copy21 = new()
			{
				Data = "3244tgsdnjvzfw9u54pngwsbf"
			};
			Game TheWitcher3 = new()
			{
				Title = "The Witcher 3: Wild Hunt",
				Images = new() { new() { Type = poster, Path = "https://m.media-amazon.com/images/W/IMAGERENDERING_521856-T1/images/I/81nBSbgcxFL._SY679_.jpg" },
					new() { Type = poster, Path = "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmHlktOHar3lVl6rY.png" } },
				Description = "The Witcher 3: Wild Hunt - це рольова гра з відкритим світом, " +
					"розроблена польською студією CD Projekt RED. Гравці беруть на себе роль відьмака Ґеральта з Рівії, який мандрує " +
					"по світу, виконуючи завдання, збираючи ресурси і розблоковуючи нові можливості. " +
					"Гра має багато сюжетних ліній, які можуть впливати на розвиток історії, а також має велику кількість " +
					"побічних завдань та активностей. В The Witcher 3: Wild Hunt також присутня система бою з мечем і магією, " +
					"а також можливості розвивати навички і збирати різні ресурси. Гра отримала численні нагороди від критиків " +
					"та гравців, зокрема, була названа кращою грою 2015 року за версією багатьох видань.",
				Released = new DateTime(2015, 5, 19),
				Genres = new List<Genre>() { RPG, action },
				Tags = new() { openWorld, fantasy, medieval, mature, violence },
				Developer = cdProjectRed,
				Publisher = cdProject,
				IsAvailable = true,
				IsHotOffer = false,
				Price = 19.99M,
				DiscountPrice = 9.9m,
				CopyType = account,
				Copies = new List<Copy>() { copy21 },
				SoldCopies = 12000000,
			};

			ImageType screenshot = new()
			{
				Name = "Screenshot"
			};

			Tag storyRich = new()
			{
				Name = "Насичена історія"
			};
			Tag shooter = new()
			{
				Name = "Шутер"
			};
			Tag characterCustomization = new()
			{
				Name = "Персоналізація персонажа"
			};
			Developer bioware = new()
			{
				Name = "BioWare"
			};
			Publisher electronicArts = new()
			{
				Name = "Electronic Arts"
			};
			Copy copy22 = new()
			{
				Data = "43852t9whegusfnijgw934gnusdf"
			};
			Game MassEffect2 = new()
			{
				Title = "Mass Effect 2",
				Images = new() {
					new()
						{
							Type = poster,
							Path = "https://upload.wikimedia.org/wikipedia/ru/9/99/MassEffect2_cover.png"
						},
					new()
						{
							Type = screenshot,
							Path = "https://s5o.ru/storage/simple/cyber/edt/9e/13/8b/9b/cybere828eb4b500.jpg"
						},
					new()
						{
							Type = screenshot,
							Path = "https://www.overclockers.ua/games/mass-effect-2/38-big-mass-effect-2.jpg"
						},
					new()
						{
							Type = screenshot,
							Path = "https://3dnews.ru/assets/external/illustrations/2017/01/05/945457/ss_c364caaae4080cf28999d12e3535b8326dff6659.1920x1080.jpg"
						},
					new()
						{
							Type = screenshot,
							Path = "https://oyster.ignimgs.com/mediawiki/apis.ign.com/mass-effect-2/3/33/Horizon_slice19.png?width=1280"
						},
					new()
						{
							Type = screenshot,
							Path = "https://assets.rpgsite.net/images/images/000/099/752/original/mass_effect_2_best_bonus_power_powers.jpg"
						}
					},
				Description = "Mass Effect 2 є відеогрою у жанрі рольової гри з видом від третьої особи, розробленої компанією BioWare та виданою Electronic Arts. Це друга гра в серії Mass Effect. Сюжет гри розповідає про зусилля головного героя, командира Шепарда, збирати свою нову команду для боротьби з загрозою реперів. Гра була випущена для Xbox 360 та Microsoft Windows у 2010 році, а пізніше для PlayStation 3 у 2011 році.",
				Released = new DateTime(2010, 1, 26),
				Genres = new List<Genre>() { RPG, action },
				Tags = new() { sciFi, storyRich, shooter, characterCustomization },
				Developer = bioware,
				Publisher = electronicArts,
				IsAvailable = true,
				IsHotOffer = true,
				Price = 19.99M,
				DiscountPrice = 15.9m,
				CopyType = key,
				Copies = new List<Copy>() { copy22 },
				SoldCopies = 7200,
			};

			Tag historical = new()
			{
				Name = "Історичний"
			};
			Copy copy23 = new()
			{
				Data = "4953twurgebsn-8354nwrbi0e"
			};
			Copy copy24 = new()
			{
				Data = "4953twurgebsn-3rqfrgavcr3244few"
			};
			Game assassinCreedValhalla = new()
			{
				Title = "Assassin's Creed Valhalla",
				Images = new() {
					new()
					{
						Type = poster,
						Path = "https://upload.wikimedia.org/wikipedia/ru/2/26/AC_Valhalla_standard_edition.jpg"
					}
				},
				Description = "Assassin's Creed Valhalla - відеогра в жанрі action-adventure та " +
					"стелс-ем'юлятор, розроблена і видана компанією Ubisoft для Microsoft Windows, " +
					"PlayStation 4, PlayStation 5, Xbox One, Xbox Series X та Series S. Гра є дванадцятим " +
					"основним випуском серії Assassin's Creed. Гравець контролює Ейвора Волкодава, вікінга " +
					"з Норвегії, який у 873 році н.е. вирушає в Англію з метою здобуття влади і створення " +
					"власного клану на території Англії. Історія гри пов'язана з легендами про вікінгів, а також " +
					"історичними подіями того періоду, такими як Велика Данська армія, завоювання та розширення " +
					"королівства Весексу, відомого сагою про Короля Артура, та іншими.",
				Released = new DateTime(2020, 11, 10),
				Genres = new List<Genre>() { action, adventure, RPG },
				Tags = new() { openWorld, historical, stealth },
				Developer = UbisoftMontreal,
				Publisher = Ubisoft,
				IsAvailable = true,
				IsHotOffer = true,
				Price = 39.99M,
				DiscountPrice = 19.9m,
				CopyType = key,
				Copies = new List<Copy>() { copy23, copy24 },
				SoldCopies = 8796552,
			};
			Genre indie = new()
			{
				Name = "Інді"
			};
			Tag sandbox = new()
			{
				Name = "Пісочниця"
			};
			Tag crafting = new()
			{
				Name = "Крафтинг"
			};
			Developer reLogic = new()
			{
				Name = "Re-Logic"
			};
			Publisher reLogicP = new()
			{
				Name = "Re-Logic"
			};
			Copy copy25 = new()
			{
				Data = "352t3j9rgeubfdn92854nrueo"
			};
			Copy copy26 = new()
			{
				Data = "452rfvdsdv54yt2g54wre"
			};
			Tag game2d = new()
			{
				Name = "2D"
			};
			Game terraria = new()
			{
				Title = "Terraria",
				Images = new() {
				new()
					{
						Type = poster,
						Path = "https://m.media-amazon.com/images/W/IMAGERENDERING_521856-T1/images/I/71k0BMp4U1L._AC_UF894,1000_QL80_.jpg"
					}
				},
				Description = "Terraria - 2D-пісочниця з відкритим світом в жанрі пригодницької відеогри. " +
					"Гра була розроблена Re-Logic та випущена на Microsoft Windows у травні 2011 року. Пізніше гра була " +
					"випущена на інші операційні системи, такі як macOS, Linux, Android, iOS, PlayStation 3, PlayStation 4, " +
					"PlayStation Vita, Xbox 360, Xbox One, Nintendo 3DS, Wii U та Nintendo Switch. Гравці можуть досліджувати " +
					"випадково згенеровані світи, збирати ресурси, виготовляти предмети та боротися зі злом. В грі є " +
					"денний/ночевий цикл, зміна погоди та різні події, які можуть відбутися у грі. Гра також має " +
					"багатокористувацький режим.",
				Released = new DateTime(2011, 5, 16),
				Genres = new List<Genre>() { adventure, indie },
				Tags = new() { openWorld, sandbox, game2d, crafting, survival },
				Developer = reLogic,
				Publisher = reLogicP,
				IsAvailable = true,
				IsHotOffer = true,
				Price = 9.99M,
				DiscountPrice = 7.9m,
				CopyType = account,
				Copies = new List<Copy>() { copy25, copy26 },
				SoldCopies = 35200000,
			};
			Tag arcade = new()
			{
				Name = "Аркада"
			};
			Developer npixel = new()
			{
				Name = "Npixel"
			};
			Publisher KakaoGames = new()
			{
				Name = "Npixel"
			};
			Tag competitive = new()
			{
				Name = "Змагання"
			};
			Developer capcom = new()
			{
				Name = "Capcom"
			};
			Publisher capcomP = new()
			{
				Name = "Capcom"
			}; ;
			Game streetFighter6 = new()
			{
				Title = "Street Fighter 6",
				Images = new() { new() { Type = poster, Path = "https://pbs.twimg.com/media/FUgPIkWXoAASe3Y.jpg:large" } },
				Description = "Street Fighter 6 - це очікувана файтинг гра, яка розробляється компанією Capcom. Гра буде наступником " +
					"Street Fighter V і буде мати більш епічні бої, оновлену графіку та багато нових персонажів. Будуть представлені " +
					"нові режими гри, зокрема, онлайн-турніри та режим гри на виживання. У грі також будуть нові механіки бою, які " +
					"дозволять гравцям виконувати більш складні комбо та спеціальні прийоми. Завдяки вдосконаленій системі онлайн-ігор, " +
					"Street Fighter 6 дозволить гравцям з усього світу змагатися між собою в захоплюючих боях.",
				Released = new DateTime(2024, 6, 30),
				Genres = new List<Genre>() { action },
				Tags = new() { multiplayer, arcade, competitive },
				Developer = capcom,
				Publisher = capcomP,
				Price = 59.9M,
				CopyType = key,
				IsAvailable = false,
				IsHotOffer = false,
				Copies = new List<Copy>() { },
				SoldCopies = 0,
			};
			Tag singlePlayer = new()
			{
				Name = "Одиночна гра",
			};
			Developer larianStudios = new()
			{
				Name = "Larian Studios"
			};
			Publisher larianStudiosP = new()
			{
				Name = "Larian Studios"
			};
			Game baldursGate3 = new()
			{
				Title = "Baldur's Gate 3",
				Images = new() { new() { Type = poster, Path = "https://upload.wikimedia.org/wikipedia/uk/c/c7/%D0%9E%D0%B1%D0%BA%D0%BB%D0%B0%D0%B4%D0%B8%D0%BD%D0%BA%D0%B0_%D0%B2%D1%96%D0%B4%D0%B5%D0%BE%D0%B3%D1%80%D0%B8_Baldur%27s_Gate_III.png" } },
				Description = "Baldur's Gate 3 - очікувана рольова гра, яка розробляється компанією Larian Studios. Гра є третьою частиною " +
					"в серії Baldur's Gate та продовжує історію світу Forgotten Realms. Гравці зможуть обрати один з численних класів героїв " +
					"та зануритися у світ фантастичної пригоди. В Baldur's Gate 3 гравці зможуть зустрітися з новими персонажами, " +
					"виконувати завдання та боротися зі своїми ворогами. Гра матиме нові механіки, які дозволять гравцям виконувати " +
					"більш складні дії, такі як взаємодія з оточуючим світом та використання магії. Будуть представлені нові режими " +
					"гри, включаючи мультиплеєр та режим гри на виживання. Завдяки вдосконаленій системі генерації ігрового світу, " +
					"Baldur's Gate 3 дозволить гравцям досліджувати світ, який ніколи не повторюється.",
				Released = new DateTime(2023, 12, 31),
				Genres = new List<Genre>() { RPG, adventure },
				Tags = new() { singlePlayer, fantasy, storyRich },
				Developer = larianStudios,
				Publisher = larianStudiosP,
				Price = 69.9M,
				CopyType = key,
				IsAvailable = false,
				IsHotOffer = false,
				Copies = new List<Copy>() { },
				SoldCopies = 0,
			};
			Developer ubisoft = new()
			{
				Name = "Ubisoft"
			};
			Game assassinCreedMirage = new()
			{
				Title = "Assassin's Creed Mirage",
				Images = new() { new() { Type = poster, Path = "https://cdna.artstation.com/p/assets/images/images/053/877/092/large/red-clown-mirage-w-logo.jpg?1663234039" } },
				Description = "Assassin's Creed Mirage - це нова гра у серії Assassin's Creed, що розробляється студією Ubisoft. Гравець зможе " +
					"побачити нову епоху - час Великого Степу, відвідати знакові міста та місцевості, такі як Самарканд та Хива, та " +
					"стати частиною світу, що належить кочівникам. В Assassin's Creed Mirage буде представлено багато нових персонажів, які " +
					"зможуть допомогти гравцеві в його пригодах. Гравець зможе відчути вільну та відкриту гру, в якій можна виконувати " +
					"багато завдань та місій. Крім того, будуть доступні різноманітні види бойової техніки та знарядь для виконання місій. " +
					"Assassin's Creed Mirage - це ідеальна гра для тих, хто любить історію, бойові мистецтва та пригоди.",
				Released = new DateTime(2023, 11, 15),
				Genres = new List<Genre>() { action, adventure },
				Tags = new() { singlePlayer, openWorld, historical },
				Developer = ubisoft,
				Publisher = Ubisoft,
				Price = 69.9M,
				CopyType = null,
				IsAvailable = false,
				IsHotOffer = false,
				Copies = new List<Copy>() { },
				SoldCopies = 0,
			};
			context.Games.AddRange(elden,
						  DoomEternal,
						  TCRSS,
						  GTA5,
						  cyberpunk2077,
						  theLastOfUs1,
						  MassEffect2,
						  TheWitcher3,
						  HorizonZeroDawn,
						  assassinCreedValhalla,
						  terraria,
						  streetFighter6,
						  baldursGate3,
						  assassinCreedMirage);
			context.SaveChanges();
		}
	}
}