CREATE DATABASE Movies
 
USE	Movies

CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY,
	GenreName NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX) 
	)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(30),
	Notes NVARCHAR(MAX)
	)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(30) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear DATETIME2 NOT NULL,
	[Length] TIME NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating DECIMAL(2, 1),
	Notes NVARCHAR(MAX)
	)

INSERT INTO Directors(DirectorName,Notes)
	VALUES
		('Frank Darabont' , NULL),
		('Francis Ford Coppola', NULL),
		('Steven Spielberg', NULL),
		('Peter Jackson', NULL),
		('Quentin Tarantino', NULL)

INSERT INTO Genres(GenreName, Notes)
	VALUES
		('Crime', NULL),
		('Action', NULL),
		('Biography', NULL),
		('Crime&Drama', NULL),
		('Drama', NULL)

INSERT INTO Categories(CategoryName,Notes)
	VALUES
		('Best Picture', NULL),
		('Best Actor in a Leading Role', NULL),
		('Best Music', NULL),
		('Best Director', NULL),
		('Best Film Editing', NULL)

INSERT INTO Movies(Title, DirectorId,CopyrightYear,
[Length],GenreId, CategoryId, Rating, Notes)
	VALUES
		('The Shawshank Redemption',1,'1994','02:11',
		5,4,9.2,NULL),
		('The Godfather',2,'1972','02:55',
		1,5,9.2, NULL),
		('Schindlers List',3,'1993','03:15',
		3,1,8.9, NULL),
		('The Lord of the Rings',
		4,'2003','03:21',
		4,2,8.9, NULL),
		('Pulp Fiction',5,'1994','02:34',
		2,3,8.9, NULL)
