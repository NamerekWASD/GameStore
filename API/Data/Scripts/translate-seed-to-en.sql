-- Translates the Ukrainian seed data (SeedData.cs, pre-i18n-4) already inserted into an
-- existing database into English, to match the translated seed used for new databases.
--
-- Needed because SeedData.Initialize only runs on an empty Games table, so editing
-- SeedData.cs has no effect on a database that was already seeded.
--
-- How to run:
--   sqlcmd -S "(localdb)\mssqllocaldb" -E -d Gamesdb -i translate-seed-to-en.sql
--   (swap -E for -U/-P, or the server name, to match your connection string)
-- or open this file in SSMS / Azure Data Studio connected to the target database and execute it,
-- or: dotnet ef database update  -- does not apply this; this script is independent of EF migrations.
--
-- Safe to run more than once: every WHERE clause matches the original Ukrainian text,
-- so rows already translated simply match zero rows on a second run.

-- Tags
UPDATE Tags SET Name = N'Open World' WHERE Name = N'Відкритий світ';
UPDATE Tags SET Name = N'First-Person Shooter' WHERE Name = N'Шутер від першої особи';
UPDATE Tags SET Name = N'Stealth' WHERE Name = N'Стелс';
UPDATE Tags SET Name = N'Action-Adventure' WHERE Name = N'Екшн-пригода';
UPDATE Tags SET Name = N'Post-Apocalyptic' WHERE Name = N'Післяапокаліптика';
UPDATE Tags SET Name = N'Multiplayer' WHERE Name = N'Багатокористувацький';
UPDATE Tags SET Name = N'Role-Playing' WHERE Name = N'Рольова гра';
UPDATE Tags SET Name = N'Open World / Sandbox' WHERE Name = N'Відкритий світ / Пісочниця';
UPDATE Tags SET Name = N'Science Fiction' WHERE Name = N'Наукова фантастика';
UPDATE Tags SET Name = N'Science Fiction' WHERE Name = N'наукова фантастика';
UPDATE Tags SET Name = N'Fantasy' WHERE Name = N'Фентезі';
UPDATE Tags SET Name = N'Crime' WHERE Name = N'Кримінальний';
UPDATE Tags SET Name = N'Racing' WHERE Name = N'Гонки';
UPDATE Tags SET Name = N'Survival' WHERE Name = N'Виживання';
UPDATE Tags SET Name = N'Horror' WHERE Name = N'Жахи';
UPDATE Tags SET Name = N'Puzzle' WHERE Name = N'Головоломки';
UPDATE Tags SET Name = N'Tactical' WHERE Name = N'Тактика';
UPDATE Tags SET Name = N'Female Protagonist' WHERE Name = N'Жінка-протагоніст';
UPDATE Tags SET Name = N'Third-Person' WHERE Name = N'Від третьої особи';
UPDATE Tags SET Name = N'Medieval' WHERE Name = N'Cередньовічний';
UPDATE Tags SET Name = N'Mature' WHERE Name = N'Зрілий';
UPDATE Tags SET Name = N'Violence' WHERE Name = N'Насильство';
UPDATE Tags SET Name = N'Story Rich' WHERE Name = N'Насичена історія';
UPDATE Tags SET Name = N'Shooter' WHERE Name = N'Шутер';
UPDATE Tags SET Name = N'Character Customization' WHERE Name = N'Персоналізація персонажа';
UPDATE Tags SET Name = N'Historical' WHERE Name = N'Історичний';
UPDATE Tags SET Name = N'Sandbox' WHERE Name = N'Пісочниця';
UPDATE Tags SET Name = N'Crafting' WHERE Name = N'Крафтинг';
UPDATE Tags SET Name = N'Arcade' WHERE Name = N'Аркада';
UPDATE Tags SET Name = N'Competitive' WHERE Name = N'Змагання';
UPDATE Tags SET Name = N'Single Player' WHERE Name = N'Одиночна гра';

-- Genres
UPDATE Genres SET Name = N'Action' WHERE Name = N'Бойовик';
UPDATE Genres SET Name = N'Tactical Shooter' WHERE Name = N'Тактичний шутер';
UPDATE Genres SET Name = N'Adventure' WHERE Name = N'Пригодницький';
UPDATE Genres SET Name = N'Indie' WHERE Name = N'Інді';

-- Platforms: all platform names in the seed (Steam, Epic Games, PlayStation 4/5) were already English.

-- CopyTypes
UPDATE CopyTypes SET Name = N'Key' WHERE Name = N'Ключ';
UPDATE CopyTypes SET Name = N'Account' WHERE Name = N'Акаунт';

-- Regions (table is called AvailableRegions in the schema)
UPDATE AvailableRegions SET Name = N'Eastern Europe' WHERE Name = N'Східна Європа';
UPDATE AvailableRegions SET Name = N'Global' WHERE Name = N'Глобальний';
UPDATE AvailableRegions SET Name = N'Asia' WHERE Name = N'Азія';

-- Games (Description), matched by Title, which was already English before this change
UPDATE Games SET Description = N'Elden Ring is a 2022 action role-playing game developed by FromSoftware and published by Bandai Namco Entertainment. Elden Ring is presented from a third-person perspective, with players freely roaming its interactive open world. Six main areas can be traversed using the steed Torrent as the primary mode of transportation, while linear, hidden dungeons can be explored to find useful items. Combat is aided by several types of weapons and magic spells, including stealth mechanics for indirect combat. Checkpoints scattered across the open world let the player improve their attributes using an in-game currency called Runes, and also act as locations for fast travel. Elden Ring features online multiplayer, letting players join one another for cooperative play and player-versus-player combat.' WHERE Title = N'Elden Ring';
UPDATE Games SET Description = N'Doom Eternal is a first-person shooter developed by id Software and published by Bethesda Softworks. It is a sequel to Doom (2016) and the seventh entry in the Doom series, released on March 20, 2020. The story picks up some time after the events of the 2016 game. It once again follows the Doom Slayer, who sets out to stop Hell''s invasion of Earth and thwart the alien Maykrs'' plans to destroy humanity.' WHERE Title = N'Doom Eternal';
UPDATE Games SET Description = N'Tom Clancy''s Rainbow Six Siege is a tactical online shooter developed by Ubisoft Montreal and published by Ubisoft. The game places a strong emphasis on environmental destruction and cooperation between players. Each player takes control of an attacker or defender in various game modes, such as hostage rescue, bomb defusal, and securing an area within a room. The title has no campaign, but includes a series of short offline missions called "situations" that can be played solo. These missions have a loose narrative centered on recruits undergoing training to prepare them for future clashes with the White Masks, a terrorist group that threatens global security.' WHERE Title = N'Tom Clancy''s Rainbow Six Siege';
UPDATE Games SET Description = N'Grand Theft Auto V (GTA V for short) is a 2013 action-adventure game developed by Rockstar North and published by Rockstar Games. It takes place in the fictional state of San Andreas, based on Southern California. The story follows three criminals as they attempt to cope with pressure from a government agency and powerful criminal figures. The open-world design lets players freely roam the San Andreas countryside and the fictional city of Los Santos, based on Los Angeles.

The game is played from either a third-person or first-person perspective, and its world is navigated on foot or by vehicle. Players control three lead protagonists throughout the single-player mode and switch between them, both during and outside missions. The story is centered around heists, and many missions involve shooting and vehicle driving. A "wanted" system governs law enforcement''s response to crimes committed by the player. Grand Theft Auto Online is an online multiplayer mode that allows up to 30 players to take part in a variety of cooperative and competitive game modes.' WHERE Title = N'Grand Theft Auto V';
UPDATE Games SET Description = N'Cyberpunk 2077 is an action RPG set in the cyberpunk genre, developed by the Polish studio CD Projekt RED on the REDengine 4 game engine. The game is an adaptation of the Cyberpunk 2020 tabletop role-playing game, set fifty-seven years later in the fictional Night City, California, featuring an open world with six distinct regions. Players take on the role of V, a mercenary whose gender and appearance are fully customizable. On one job, V becomes an unwilling witness to the murder of the head of a corporation that created a chip capable of transferring consciousness. V is framed for the murder and shot. But thanks to the chip, V survives and sets out to restore justice, all while searching for a way to get rid of the unexpected side effects of carrying the chip.' WHERE Title = N'Cyberpunk 2077';
UPDATE Games SET Description = N'The Last of Us Part I is an action-adventure game developed by Naughty Dog and published by Sony Interactive Entertainment for PlayStation 5 in September 2022. A Microsoft Windows port followed in March 2023. It is a remake of The Last of Us (2013) featuring a reworked gameplay experience, including improved combat mechanics and environmental exploration, as well as expanded accessibility options. The story follows Joel, who must escort a teenage girl, Ellie, across a post-apocalyptic United States and defend her from cannibalistic creatures infected by a mutated strain of the cordyceps fungus. The game includes the Left Behind DLC, which tells the story of Ellie and her best friend Riley. The original game''s multiplayer mode was not included in the remake.' WHERE Title = N'The Last of Us Part I';
UPDATE Games SET Description = N'Horizon Zero Dawn is an action RPG developed by Guerrilla Games and published by Sony Interactive Entertainment for PlayStation 4 in 2017, and for PC in 2020. The game takes place in an open world where the player takes on the role of Aloy, a young hunter from the Nora tribe. The story follows Aloy''s origins as she lives in a world ruled by machines, where humanity has fallen from its former path. Her task is to explore the world, fight the machines, and find answers to difficult questions.' WHERE Title = N'Horizon Zero Dawn';
UPDATE Games SET Description = N'The Witcher 3: Wild Hunt is an open-world role-playing game developed by the Polish studio CD Projekt RED. Players take on the role of witcher Geralt of Rivia, who travels across the world completing quests, gathering resources, and unlocking new abilities. The game features many storylines that can affect how the plot unfolds, as well as a large number of side quests and activities. The Witcher 3: Wild Hunt also features a sword-and-magic combat system, along with the ability to develop skills and gather various resources. The game received numerous awards from critics and players alike, including being named Game of the Year for 2015 by many publications.' WHERE Title = N'The Witcher 3: Wild Hunt';
UPDATE Games SET Description = N'Mass Effect 2 is a third-person role-playing game developed by BioWare and published by Electronic Arts. It is the second game in the Mass Effect series. The story follows the efforts of the protagonist, Commander Shepard, to assemble a new team to fight the threat of the Reapers. The game was released for Xbox 360 and Microsoft Windows in 2010, and later for PlayStation 3 in 2011.' WHERE Title = N'Mass Effect 2';
UPDATE Games SET Description = N'Assassin''s Creed Valhalla is an action-adventure game with stealth elements, developed and published by Ubisoft for Microsoft Windows, PlayStation 4, PlayStation 5, Xbox One, Xbox Series X, and Series S. It is the twelfth main installment in the Assassin''s Creed series. The player controls Eivor Wolf-Kissed, a Viking from Norway who, in 873 AD, journeys to England seeking power and to build a clan of his own on English soil. The story is tied to Viking legends, as well as historical events of the period, such as the Great Heathen Army, the conquest and expansion of the Kingdom of Wessex, famed in the legend of King Arthur, and others.' WHERE Title = N'Assassin''s Creed Valhalla';
UPDATE Games SET Description = N'Terraria is a 2D open-world sandbox adventure game. It was developed by Re-Logic and released on Microsoft Windows in May 2011. The game was later released on other platforms, including macOS, Linux, Android, iOS, PlayStation 3, PlayStation 4, PlayStation Vita, Xbox 360, Xbox One, Nintendo 3DS, Wii U, and Nintendo Switch. Players can explore randomly generated worlds, gather resources, craft items, and fight evil. The game features a day/night cycle, changing weather, and various events that can occur during play. It also has a multiplayer mode.' WHERE Title = N'Terraria';
UPDATE Games SET Description = N'Street Fighter 6 is a highly anticipated fighting game being developed by Capcom. It is the successor to Street Fighter V and will feature more epic battles, updated graphics, and many new characters. It will introduce new game modes, including online tournaments and a survival mode. The game will also feature new combat mechanics that let players pull off more complex combos and special moves. Thanks to an improved online system, Street Fighter 6 will let players from around the world compete against each other in thrilling battles.' WHERE Title = N'Street Fighter 6';
UPDATE Games SET Description = N'Baldur''s Gate 3 is a highly anticipated role-playing game being developed by Larian Studios. It is the third installment in the Baldur''s Gate series and continues the story of the Forgotten Realms world. Players will be able to choose from numerous hero classes and immerse themselves in a world of fantastical adventure. In Baldur''s Gate 3, players will be able to meet new characters, complete quests, and fight their enemies. The game will feature new mechanics that let players perform more complex actions, such as interacting with the surrounding world and using magic. New game modes will be introduced, including multiplayer and a survival mode. Thanks to an improved world-generation system, Baldur''s Gate 3 will let players explore a world that never repeats itself.' WHERE Title = N'Baldur''s Gate 3';
UPDATE Games SET Description = N'Assassin''s Creed Mirage is a new entry in the Assassin''s Creed series, being developed by Ubisoft. Players will be able to experience a new era - the time of the Great Steppe - and visit iconic cities and locations such as Samarkand and Khiva, becoming part of a world that belongs to nomads. Assassin''s Creed Mirage will introduce many new characters who will help the player on their adventures. Players will be able to enjoy a free and open game in which they can complete many quests and missions. In addition, a variety of combat gear and tools will be available for completing missions. Assassin''s Creed Mirage is the perfect game for those who love history, martial arts, and adventure.' WHERE Title = N'Assassin''s Creed Mirage';

-- Note: "Grand Theft Auto VI" and the refreshed release dates for the not-yet-released
-- games (Street Fighter 6, Baldur's Gate 3, Assassin's Creed Mirage) are new seed content,
-- not translations of existing rows, so they are not part of this script. Re-seed a fresh
-- database (drop it and let SeedData.Initialize run again) to pick those up.
