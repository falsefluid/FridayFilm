CREATE Database dbFridayFilmClub;

USE dbFridayFilmClub;

SELECT * FROM Users;

SELECT * FROM Reviews;

SELECT * FROM Genres;

SELECT * FROM Credits;

SELECT * FROM Celebrities;

SELECT * FROM CelebritiesInMovies;

SELECT * FROM Movies;

SELECT * FROM MovieGenres;

INSERT INTO Movies(MovieTitle, Year, Rated, RunTime, Plot, Poster, ReleaseDate, Trailer)
VALUES
('F9: The Fast Saga', 2021, 'PG-13', 143, 'Dom and the crew must take on an international terrorist who turns out to be Dom and Mia''s estranged brother.', 'https://m.media-amazon.com/images/M/MV5BODJkMTQ5ZmQtNzQxYy00ZWNlLWI0ZGYtYjU1NzdiMjcyNDRmXkEyXkFqcGc@._V1_SX300.jpg', '2021/06/25',  'https://www.youtube.com/watch?v=FUK2kdPsBws&ab_channel=UniversalPicturesUK'),
('Interstellar', 2014, 'PG-13', 169, 'When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.', 'https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_SX300.jpg', '2014/11/07', 'https://www.youtube.com/watch?v=zSWdZVtXT7E&ab_channel=WarnerBros.UK%26Ireland'),
('The Dark Knight', 2008, 'PG-13', 152, 'When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end to the madness.', 'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_SX300.jpg', '2008/07/18', 'https://www.youtube.com/watch?v=LDG9bisJEaI&ab_channel=DC'),
('Free Guy', 2021, 'PG-13', 115, 'When Guy, a bank teller, learns that he is a non-player character in a bloodthirsty, open-world video game, he goes on to become the hero of the story and takes the responsibility of saving the world.', 'https://m.media-amazon.com/images/M/MV5BN2I0MGMxYjUtZTZiMS00MzMxLTkzNWYtMDUyZmUwY2ViYTljXkEyXkFqcGc@._V1_SX300.jpg', '2021/08/13', 'https://www.youtube.com/watch?app=desktop&v=zuttUdbnemA&ts&ab_channel=20thCenturyStudiosUK'),
('Mufasa: The Lion King', 2024, 'PG', 118, 'Mufasa, a cub lost and alone, meets a sympathetic lion named Taka, the heir to a royal bloodline. The chance meeting sets in motion an expansive journey of a group of misfits searching for their destiny.', 'https://m.media-amazon.com/images/M/MV5BYjBkOWUwODYtYWI3YS00N2I0LWEyYTktOTJjM2YzOTc3ZDNlXkEyXkFqcGc@._V1_SX300.jpg', '2024/12/20', 'https://www.youtube.com/watch?v=o17MF9vnabg&ab_channel=Disney'),
('Sonic the Hedgehog 3', 2024, 'PG', 110, 'Sonic, Knuckles, and Tails reunite against a powerful new adversary, Shadow, a mysterious villain with powers unlike anything they have faced before. With their abilities outmatched, Team Sonic must seek out an unlikely alliance.', 'https://m.media-amazon.com/images/M/MV5BMjZjNjE5NDEtOWJjYS00Mjk2LWI1ZDYtOWI1ZWI3MzRjM2UzXkEyXkFqcGc@._V1_SX300.jpg', '2024/12/20', 'https://www.youtube.com/watch?v=qSu6i2iFMO0&ab_channel=ParamountPictures'),
('Inception', 2010, 'PG-13', 148, 'A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.', 'https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_SX300.jpg', '2010/07/16', 'https://www.youtube.com/watch?v=YoHD9XEInc0&ab_channel=RottenTomatoesClassicTrailers'),
('Nosferatu', 2024, 'R', 132, 'A gothic tale of obsession between a haunted young woman and the terrifying vampire infatuated with her, causing untold horror in its wake.', 'https://m.media-amazon.com/images/M/MV5BY2FhZGE3NmEtNWJjOC00NDI1LWFhMTQtMjcxNmQzZmEwNGIzXkEyXkFqcGc@._V1_SX300.jpg', '2024/12/25', 'https://www.youtube.com/watch?v=Px6S0RxfAHg&ab_channel=UniversalPicturesUK'),
('Avengers: Endgame', 2019, 'PG-13', 181, 'After the devastating events of Avengers: Infinity War (2018), the universe is in ruins. With the help of remaining allies, the Avengers assemble once more in order to reverse Thanos'' actions and restore balance to the universe.', 'https://m.media-amazon.com/images/M/MV5BMTc5MDE2ODcwNV5BMl5BanBnXkFtZTgwMzI2NzQ2NzM@._V1_SX300.jpg', '2019/04/26', 'https://www.youtube.com/watch?v=wuOvmyuYFMo&ab_channel=MarvelUK'),
('Deadpool & Wolverine', 2024, 'R', 128, 'Deadpool is offered a place in the Marvel Cinematic Universe by the Time Variance Authority, but instead recruits a variant of Wolverine to save his universe from extinction.', 'https://m.media-amazon.com/images/M/MV5BZTk5ODY0MmQtMzA3Ni00NGY1LThiYzItZThiNjFiNDM4MTM3XkEyXkFqcGc@._V1_SX300.jpg', '2024/07/26', 'https://www.youtube.com/watch?v=73_1biulkYk&ab_channel=MarvelEntertainment'),
('Spider-Man: Across the Spider-Verse', 2023, 'PG', 140, 'Miles Morales catapults across the multiverse, where he encounters a team of Spider-People charged with protecting its very existence. When the heroes clash on how to handle a new threat, Miles must redefine what it means to be a hero.', 'https://m.media-amazon.com/images/M/MV5BNThiZjA3MjItZGY5Ni00ZmJhLWEwN2EtOTBlYTA4Y2E0M2ZmXkEyXkFqcGc@._V1_SX300.jpg', '2023/06/02', 'https://www.youtube.com/watch?v=cqGjhVJWtEg&ab_channel=SonyPicturesEntertainment'),
('Dragon Ball Super: Broly', 2018, 'PG', 100, 'Goku and Vegeta encounter Broly, a Saiyan warrior unlike any fighter they''ve faced before.', 'https://m.media-amazon.com/images/M/MV5BMTA5MTc1M2EtZWQ2Ni00ZmU2LTg3MzQtOTliMjE4OGM0ZWFiXkEyXkFqcGc@._V1_SX300.jpg', '2019/01/16', 'https://www.youtube.com/watch?v=FHgm89hKpXU&ab_channel=IGN'),
('The Truman Show', 1998, 'PG', '103', 'An insurance salesman begins to suspect that his whole life is actually some sort of reality TV show.', 'https://m.media-amazon.com/images/M/MV5BNzA3ZjZlNzYtMTdjMy00NjMzLTk5ZGYtMTkyYzNiOGM1YmM3XkEyXkFqcGc@._V1_SX300.jpg', '1998/06/05', 'https://www.youtube.com/watch?v=dlnmQbPGuls'),
('John Wick: Chapter 4', 2023, 'R', 169,'John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.', 'https://m.media-amazon.com/images/M/MV5BY2Q2ZmI5ZjUtNWVhMC00YzJkLTlmYjMtY2RmZDhkNzEzYjZhXkEyXkFqcGc@._V1_SX300.jpg', '2023/03/24', 'https://www.youtube.com/watch?v=qEVUtrk8_B4');

INSERT INTO MovieGenres (MovieID, GenreID) 
VALUES
(1, 1), -- F9: The Fast Saga - Action
(1, 5), -- F9: The Fast Saga - Crime
(1, 12), -- F9: The Fast Saga - Thriller
(2, 11), -- Interstellar - Sci-Fi
(2, 6), -- Interstellar - Drama
(3, 1), -- The Dark Knight - Action
(3, 5), -- The Dark Knight - Crime
(3, 6), -- The Dark Knight - Drama
(4, 1), -- Free Guy - Action
(4, 3), -- Free Guy - Adventure
(4, 4), -- Free Guy - Comedy
(5, 2), -- Mufasa: The Lion King - Animation
(5, 3), -- Mufasa: The Lion King - Adventure
(5, 6), -- Mufasa: The Lion King - Drama
(6, 1), -- Sonic the Hedgehog 3 - Action
(6, 3), -- Sonic the Hedgehog 3 - Adventure
(6, 4), -- Sonic the Hedgehog 3 - Comedy
(7, 1), -- Inception - Action
(7, 3), -- Inception - Adventure
(7, 11), -- Inception - Sci-Fi
(8, 8), -- Nosferatu - Fantasy
(8, 9), -- Nosferatu - Horror
(8, 7), -- Nosferatu - Documentary
(9, 1), -- Avengers: Endgame - Action
(9, 3), -- Avengers: Endgame - Adventure
(9, 6), -- Avengers: Endgame - Drama
(10, 1), -- Deadpool & Wolverine - Action
(10, 3), -- Deadpool & Wolverine - Adventure
(10, 4), -- Deadpool & Wolverine - Comedy
(11, 2), -- Spider-Man: Across the Spider-Verse - Animation
(11, 1), -- Spider-Man: Across the Spider-Verse - Action
(11, 3), -- Spider-Man: Across the Spider-Verse - Adventure
(12, 2), -- Dragon Ball Super: Broly - Animation
(12, 1), -- Dragon Ball Super: Broly - Action
(12, 3), -- Dragon Ball Super: Broly - Adventure
(13, 4), -- The Truman Show - Comedy
(13, 6), -- The Truman Show - Drama
(14, 1), -- John Wick: Chapter 4 - Action
(14, 5), -- John Wick: Chapter 4 - Crime
(14, 12); -- John Wick: Chapter 4 - Thriller

INSERT INTO Celebrities(Forename, Surname, Bio, Birthday, Birthplace, Picture) 
VALUES
('Jim', 'Carrey', 'Jim Carrey is a Canadian-American actor, comedian, writer, and artist, known for his energetic performances and versatility in both comedic and dramatic roles.', '17 January 1962', 'Newmarket, Ontario, Canada', 'https://resizing.flixster.com/XPmnCzYFjS3XMHfGkyfNA37oZFU=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/128650_v9_bd.jpg'),
('Ben', 'Schwartz', 'Ben Schwartz is an American actor, comedian, writer, and producer, best known for his role in "Parks and Recreation" and as the voice of Sonic the Hedgehog in the "Sonic the Hedgehog" films.', '15 September 1981', 'The Bronx, New York, USA', 'https://resizing.flixster.com/EzgfVRxXwHcDlqY7_BV7MnqvxyU=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/421779_v9_bb.jpg'),
('Keanu', 'Reeves', 'Keanu Reeves is a Canadian actor, director, and producer, widely recognized for his roles in "The Matrix" series and "John Wick." He is known for his kindness and humility.', '2 September 1964', 'Beirut, Lebanon', 'https://resizing.flixster.com/hSIF9fRxVUQB6p-S9KY3RRBh22A=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/1443_v9_bc.jpg'),
('James', 'Marsden', 'James Marsden is an American actor, singer, and former model, known for his roles in "X-Men," "The Notebook," and "Westworld."', '18 September 1973', 'Stillwater, Oklahoma, USA', 'https://resizing.flixster.com/fTPKNRM0orJZA5nBnpt9d6i-ggw=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/68628_v9_bc.jpg'),
('Tika', 'Sumpter', 'Tika Sumpter is an American actress, producer, and singer, known for her roles in "Gossip Girl" and "The Haves and the Have Nots."', '20 June 1980', 'New York City, New York, USA', 'https://resizing.flixster.com/yCVtlidzjlsh7SBoyR0USKWy3OY=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/492529_v9_bc.jpg'),
('Idris', 'Elba', 'Idris Elba is a British actor, producer, and musician, famous for his roles in "Luther," "Thor," and "The Wire."', '6 September 1972', 'London, England, UK', 'https://resizing.flixster.com/4auiAaS0_m87qmsPA2Htu8RjfpI=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/178333_v9_bd.jpg'),
('Jeff', 'Fowler', 'Jeff Fowler is an American director and animator, best known for directing "Sonic the Hedgehog" (2020) and its sequel.', '27 July 1978', 'Normal, Illinois, USA', 'https://resizing.flixster.com/lqP0QBRL4zIuPf5kAiR7-ZKiEhg=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/520517_v9_aa.jpg'),
('Colleen', 'O''Shaughnessey', 'Colleen O''Shaughnessey is an American voice actress, best known for her roles as Tails in the "Sonic the Hedgehog" films and various animated TV series, including "Naruto" and "Danny Phantom."', '15 September 1971', 'Grand Rapids, Michigan, USA', 'https://resizing.flixster.com/mHmgKhpu1clkQvCgvBFNEZzkrX8=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/406947_v9_aa.jpg'),
('Adam', 'Pally', 'Adam Pally is an American actor, comedian, and writer, known for his roles in "Happy Endings," "The Mindy Project," and "Sonic the Hedgehog."', '18 March 1982', 'New York City, New York, USA', 'https://resizing.flixster.com/C4GqYx2ywjGTsw8gbOiUWtA0Bqk=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/527848_v9_bc.jpg'),
('Lee', 'Majdoub', 'Lee Majdoub is a Canadian actor, widely recognized for his role as Agent Stone in the "Sonic the Hedgehog" films and appearances in TV series such as "The 100" and "Supernatural."', '31 May 1982', 'Tripoli, Lebanon', 'https://resizing.flixster.com/3q5sWfwMPFDITOF0EpXSK6cK1wk=/218x280/v2/https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/632465_v9_ba.jpg');


-- Sonic Movie / Characters
INSERT INTO CelebritiesInMovies(MovieID, CelebrityID, CreditID, Character)
VALUES
(6, 1, 2, 'Ivo Robotnik'),
(6, 2, 9, 'Sonic'),
(6, 3, 9, 'Shadow'),
(6, 4, 2, 'Tom'),
(6, 5, 2, 'Maddie'),
(6, 6, 9, 'Knuckles'),
(6, 8, 9, 'Tails'),
(6, 9, 2, 'Wade'),
(6, 10, 2, 'Agent Stone');


-- Sonic Movie / Film Crew
INSERT INTO CelebritiesInMovies(MovieID, CelebrityID, CreditID)
VALUES
(6, 7, 1);

-- The Truman Show / Characters
INSERT INTO CelebritiesInMovies(MovieID, CelebrityID, CreditID, Character)
VALUES
(13, 1, 2, 'Truman Burbank')

-- John Wick / Characters
INSERT INTO CelebritiesInMovies(MovieID, CelebrityID, CreditID, Character)
VALUES
(14, 3, 2, 'John Wick');