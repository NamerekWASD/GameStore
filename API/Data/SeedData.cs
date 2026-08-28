using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new GameContext(serviceProvider.GetRequiredService<DbContextOptions<GameContext>>());
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            if (userManager != null)
            {
                InsertAdmin(userManager, roleManager);
            }
            // Look for any movies.
            if (context.Games.Any())
            {
                return;
            }

            Tag openWorld = new()
            {
                Name = "Open World"
            };

            Tag firstPersonShooter = new()
            {
                Name = "First-Person Shooter"
            };

            Tag stealth = new()
            {
                Name = "Stealth"
            };

            Tag actionAdventure = new()
            {
                Name = "Action-Adventure"
            };

            Tag postApocalyptic = new()
            {
                Name = "Post-Apocalyptic"
            };

            Tag multiplayer = new()
            {
                Name = "Multiplayer"
            };

            Tag rolePlaying = new()
            {
                Name = "Role-Playing"
            };

            Tag openWorldSandbox = new()
            {
                Name = "Open World / Sandbox"
            };

            Tag scienceFiction = new()
            {
                Name = "Science Fiction"
            };

            Tag fantasy = new()
            {
                Name = "Fantasy"
            };

            Tag crime = new()
            {
                Name = "Crime"
            };

            Tag racing = new()
            {
                Name = "Racing"
            };

            Tag survival = new()
            {
                Name = "Survival"
            };

            Tag horror = new()
            {
                Name = "Horror"
            };

            Tag puzzle = new()
            {
                Name = "Puzzle"
            };
            Tag tactical = new()
            {
                Name = "Tactical"
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
                Name = "Action"
            };

            var EastEurope = new List<Region>()
            {
                new Region()
                {
                    Name = "Eastern Europe"
                }
            };
            CopyType key = new()
            {
                Name = "Key",
                Platform = steam,
                AvailableRegions = EastEurope,
            };

            ImageType poster = new()
            {
                Name = Constants.POSTER_IMAGE,
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
                Description = "Elden Ring is a 2022 action role-playing game developed by FromSoftware " +
                "and published by Bandai Namco Entertainment. Elden Ring is presented from a " +
                "third-person perspective, with players freely roaming its interactive open world. Six main " +
                "areas can be traversed using the steed Torrent as the primary mode of transportation, " +
                "while linear, hidden dungeons can be explored to find useful items. Combat is aided " +
                "by several types of weapons and magic spells, including stealth mechanics for indirect combat. " +
                "Checkpoints scattered across the open world let the player improve their " +
                "attributes using an in-game currency called Runes, and also act as locations for fast " +
                "travel. Elden Ring features online multiplayer, letting players join one another " +
                "for cooperative play and player-versus-player combat.",
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
                Description = "Doom Eternal is a first-person shooter developed by id Software " +
                "and published by Bethesda Softworks. It is a sequel to Doom (2016) and the seventh entry in the Doom series, released on March 20, 2020. " +
                "The story picks up some time after the events of the 2016 game. It once again follows " +
                "the Doom Slayer, who sets out to stop Hell's invasion of Earth and thwart the alien " +
                "Maykrs' plans to destroy humanity.",
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
                Name = "Tactical Shooter",
            };

            CopyType account = new()
            {
                Name = "Account",
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
                Description = "Tom Clancy's Rainbow Six Siege is a tactical online shooter " +
                "developed by Ubisoft Montreal and published by Ubisoft. The game places a strong emphasis on " +
                "environmental destruction and cooperation between players. Each player takes " +
                "control of an attacker or defender in various game modes, such as hostage " +
                "rescue, bomb defusal, and securing an area within a room. The title has no campaign, but includes " +
                "a series of short offline missions called \"situations\" that can be played solo. These missions have " +
                "a loose narrative centered on recruits undergoing training to prepare them for future clashes " +
                "with the White Masks, a terrorist group that threatens global security.",
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
                Name = "Global"
            };

            Platform epicGames = new()
            {
                Name = "Epic Games"
            };

            Genre adventure = new()
            {
                Name = "Adventure",
            };

            Genre TPS = new()
            {
                Name = "TPS",
            };

            CopyType accountEpicGames = new()
            {
                Name = "Account",
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
                Description = "Grand Theft Auto V (GTA V for short) is a 2013 action-adventure game developed " +
                "by Rockstar North and published by Rockstar Games. It takes place in the fictional state of San Andreas, " +
                "based on Southern California. The story follows three criminals " +
                "as they attempt to cope with pressure from a government agency and powerful criminal figures. The open-world design " +
                "lets players freely roam the San Andreas countryside and the fictional city of Los Santos, based on Los Angeles." +
                "\r\n\r\nThe game is played from either a third-person or first-person perspective, and its world is navigated on foot or by vehicle. " +
                "Players control three lead protagonists throughout the single-player mode and switch between them, both during " +
                "and outside missions. The story is centered around heists, and many missions involve shooting and vehicle driving. " +
                "A \"wanted\" system governs law enforcement's response to crimes committed by the player. Grand Theft Auto Online is an online multiplayer mode " +
                "that allows up to 30 players to take part in a variety of cooperative and competitive game modes.",
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
                Description = "Cyberpunk 2077 is an action RPG set in the cyberpunk genre, developed by the Polish studio CD Projekt RED on the REDengine 4 game engine. " +
                "The game is an adaptation of the Cyberpunk 2020 tabletop role-playing game, " +
                "set fifty-seven years later in the fictional " +
                "Night City, California, featuring an open world with six distinct regions. " +
                "Players take on the role of V, a mercenary whose gender and appearance are fully " +
                "customizable. On one job, V becomes an unwilling witness to the murder of the head of a corporation " +
                "that created a chip capable of transferring consciousness. V is framed for the murder and shot. " +
                "But thanks to the chip, V survives and sets out to restore justice, all while searching for a way to get rid of " +
                "the unexpected side effects of carrying the chip.",
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
                Name = "Account",
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
                Description = "The Last of Us Part I is an action-adventure game " +
                "developed by Naughty Dog and published by Sony Interactive Entertainment for PlayStation 5 " +
                "in September 2022. A Microsoft Windows port followed in March 2023. " +
                "It is a remake of The Last of Us (2013) featuring a reworked gameplay experience, including improved " +
                "combat mechanics and environmental exploration, as well as expanded accessibility options. The story " +
                "follows Joel, who must escort a teenage girl, Ellie, across a post-apocalyptic United States " +
                "and defend her from cannibalistic creatures infected by a mutated strain of the cordyceps fungus. The game includes " +
                "the Left Behind DLC, which tells the story of Ellie and her best friend Riley. The original game's multiplayer mode " +
                "was not included in the remake.",
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
                Name = "Science Fiction"
            };
            Tag femaleProtagonist = new()
            {
                Name = "Female Protagonist"
            }; Tag thirdPerson = new()
            {
                Name = "Third-Person"
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
                Name = "Asia"
            };

            CopyType PS4Account = new()
            {
                Name = "Account",
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
                Description = "Horizon Zero Dawn is an action RPG developed by Guerrilla Games " +
                    "and published by Sony Interactive Entertainment for PlayStation 4 in 2017, and for PC in 2020. The game takes place " +
                    "in an open world where the player takes on the role of Aloy, a young hunter from the Nora tribe. The story follows " +
                    "Aloy's origins as she lives in a world ruled by machines, where humanity has fallen from its former path. Her task is to explore the world, " +
                    "fight the machines, and find answers to difficult questions.",
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
                Name = "Medieval"
            };
            Tag mature = new()
            {
                Name = "Mature"
            };
            Tag violence = new()
            {
                Name = "Violence"
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
                Description = "The Witcher 3: Wild Hunt is an open-world role-playing game " +
                    "developed by the Polish studio CD Projekt RED. Players take on the role of witcher Geralt of Rivia, who travels " +
                    "across the world completing quests, gathering resources, and unlocking new abilities. " +
                    "The game features many storylines that can affect how the plot unfolds, as well as a large number of " +
                    "side quests and activities. The Witcher 3: Wild Hunt also features a sword-and-magic combat system, " +
                    "along with the ability to develop skills and gather various resources. The game received numerous awards from critics " +
                    "and players alike, including being named Game of the Year for 2015 by many publications.",
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
                Name = "Story Rich"
            };
            Tag shooter = new()
            {
                Name = "Shooter"
            };
            Tag characterCustomization = new()
            {
                Name = "Character Customization"
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
                Description = "Mass Effect 2 is a third-person role-playing game developed by BioWare and published by Electronic Arts. It is the second game in the Mass Effect series. The story follows the efforts of the protagonist, Commander Shepard, to assemble a new team to fight the threat of the Reapers. The game was released for Xbox 360 and Microsoft Windows in 2010, and later for PlayStation 3 in 2011.",
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
                Name = "Historical"
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
                Description = "Assassin's Creed Valhalla is an action-adventure game with " +
                    "stealth elements, developed and published by Ubisoft for Microsoft Windows, " +
                    "PlayStation 4, PlayStation 5, Xbox One, Xbox Series X, and Series S. It is the twelfth " +
                    "main installment in the Assassin's Creed series. The player controls Eivor Wolf-Kissed, a Viking " +
                    "from Norway who, in 873 AD, journeys to England seeking power and to build " +
                    "a clan of his own on English soil. The story is tied to Viking legends, as well as " +
                    "historical events of the period, such as the Great Heathen Army, the conquest and expansion of " +
                    "the Kingdom of Wessex, famed in the legend of King Arthur, and others.",
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
                Name = "Indie"
            };
            Tag sandbox = new()
            {
                Name = "Sandbox"
            };
            Tag crafting = new()
            {
                Name = "Crafting"
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
                Description = "Terraria is a 2D open-world sandbox adventure game. " +
                    "It was developed by Re-Logic and released on Microsoft Windows in May 2011. The game was later " +
                    "released on other platforms, including macOS, Linux, Android, iOS, PlayStation 3, PlayStation 4, " +
                    "PlayStation Vita, Xbox 360, Xbox One, Nintendo 3DS, Wii U, and Nintendo Switch. Players can explore " +
                    "randomly generated worlds, gather resources, craft items, and fight evil. The game features a " +
                    "day/night cycle, changing weather, and various events that can occur during play. It also has a " +
                    "multiplayer mode.",
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
                Name = "Arcade"
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
                Name = "Competitive"
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
                Description = "Street Fighter 6 is a highly anticipated fighting game being developed by Capcom. It is the successor to " +
                    "Street Fighter V and will feature more epic battles, updated graphics, and many new characters. It will introduce " +
                    "new game modes, including online tournaments and a survival mode. The game will also feature new combat mechanics that " +
                    "let players pull off more complex combos and special moves. Thanks to an improved online system, " +
                    "Street Fighter 6 will let players from around the world compete against each other in thrilling battles.",
                Released = DateTime.UtcNow.AddMonths(3),
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
                Name = "Single Player",
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
                Description = "Baldur's Gate 3 is a highly anticipated role-playing game being developed by Larian Studios. It is the third installment " +
                    "in the Baldur's Gate series and continues the story of the Forgotten Realms world. Players will be able to choose from numerous hero classes " +
                    "and immerse themselves in a world of fantastical adventure. In Baldur's Gate 3, players will be able to meet new characters, " +
                    "complete quests, and fight their enemies. The game will feature new mechanics that let players perform " +
                    "more complex actions, such as interacting with the surrounding world and using magic. New game modes will be introduced, " +
                    "including multiplayer and a survival mode. Thanks to an improved world-generation system, " +
                    "Baldur's Gate 3 will let players explore a world that never repeats itself.",
                Released = DateTime.UtcNow.AddMonths(6),
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
                Description = "Assassin's Creed Mirage is a new entry in the Assassin's Creed series, being developed by Ubisoft. Players will be able " +
                    "to experience a new era - the time of the Great Steppe - and visit iconic cities and locations such as Samarkand and Khiva, " +
                    "becoming part of a world that belongs to nomads. Assassin's Creed Mirage will introduce many new characters who " +
                    "will help the player on their adventures. Players will be able to enjoy a free and open game in which they can complete " +
                    "many quests and missions. In addition, a variety of combat gear and tools will be available for completing missions. " +
                    "Assassin's Creed Mirage is the perfect game for those who love history, martial arts, and adventure.",
                Released = DateTime.UtcNow.AddMonths(9),
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

            Game gta6 = new()
            {
                Title = "Grand Theft Auto VI",
                Images = new() { new() { Type = poster, Path = "https://upload.wikimedia.org/wikipedia/en/4/46/Grand_Theft_Auto_VI.png" } },
                Description = "Grand Theft Auto VI is an upcoming action-adventure game developed by Rockstar North " +
                    "and published by Rockstar Games. It returns to the state of Leonida, including a fictionalized " +
                    "version of Miami, and follows the story of Lucia and her partner as they pursue the American dream " +
                    "through a life of crime. The game continues the series' open-world design, letting players freely " +
                    "explore its world on foot or by vehicle, and is expected to feature the series' largest and most " +
                    "detailed world to date.",
                Released = DateTime.UtcNow.AddMonths(12),
                Genres = new List<Genre>() { action, adventure, TPS },
                Tags = new() { openWorld, crime, multiplayer },
                Developer = rockStar,
                Publisher = rockstarGames,
                Price = 69.9M,
                CopyType = null,
                IsAvailable = false,
                IsHotOffer = true,
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
                          assassinCreedMirage,
                          gta6);




            context.SaveChanges();
        }

        private static void InsertAdmin(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            var admin = userManager.FindByNameAsync("admin").Result;
            if (admin != null)
            {
                return;
            }

            admin = new User
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            var result = userManager.CreateAsync(admin).Result;

            if (result.Succeeded)
            {
                if (!roleManager.RoleExistsAsync(Constants.ADMINISTRATOR).Result)
                {
                    result = roleManager.CreateAsync(new IdentityRole<int>(Constants.ADMINISTRATOR)).Result;
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.WriteLine($"Failed to create admin role: {errors}");
                }
                result = userManager.AddToRoleAsync(admin, Constants.ADMINISTRATOR).Result;
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"Failed to create admin user: {errors}");
            }

        }
    }
}