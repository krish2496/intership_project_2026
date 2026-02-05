-- Quick script to add sample anime to database
-- Run this in your database to populate it with anime

INSERT INTO "Media" ("Title", "Type", "TotalEpisodes", "Description", "CoverImageUrl", "ExternalId")
VALUES 
('Attack on Titan', 0, 75, 'Humanity fights for survival against giant humanoid creatures called Titans.', 'https://cdn.myanimelist.net/images/anime/10/47347.jpg', 16498),
('Death Note', 0, 37, 'A high school student discovers a supernatural notebook that allows him to kill anyone.', 'https://cdn.myanimelist.net/images/anime/9/9453.jpg', 1535),
('Fullmetal Alchemist: Brotherhood', 0, 64, 'Two brothers search for the Philosopher''s Stone to restore their bodies.', 'https://cdn.myanimelist.net/images/anime/1223/96541.jpg', 5114),
('Steins;Gate', 0, 24, 'A group of friends discover time travel through a microwave.', 'https://cdn.myanimelist.net/images/anime/5/73199.jpg', 9253),
('One Punch Man', 0, 24, 'A hero who can defeat any opponent with a single punch seeks a worthy challenge.', 'https://cdn.myanimelist.net/images/anime/12/76049.jpg', 30276),
('Demon Slayer', 0, 26, 'A boy becomes a demon slayer to avenge his family and cure his sister.', 'https://cdn.myanimelist.net/images/anime/1286/99889.jpg', 38000),
('My Hero Academia', 0, 113, 'A boy born without superpowers dreams of becoming a hero.', 'https://cdn.myanimelist.net/images/anime/10/78745.jpg', 31964),
('Naruto', 0, 220, 'A young ninja seeks recognition and dreams of becoming the Hokage.', 'https://cdn.myanimelist.net/images/anime/13/17405.jpg', 20),
('Sword Art Online', 0, 25, 'Players trapped in a virtual reality MMORPG must clear the game to escape.', 'https://cdn.myanimelist.net/images/anime/11/39717.jpg', 11757),
('Code Geass', 0, 50, 'An exiled prince gains the power to control minds and leads a rebellion.', 'https://cdn.myanimelist.net/images/anime/5/50331.jpg', 1575),
('Hunter x Hunter', 0, 148, 'A young boy searches for his father and becomes a Hunter.', 'https://cdn.myanimelist.net/images/anime/11/33657.jpg', 11061),
('Tokyo Ghoul', 0, 12, 'A college student becomes a half-ghoul and struggles to survive.', 'https://cdn.myanimelist.net/images/anime/5/64449.jpg', 22319);
