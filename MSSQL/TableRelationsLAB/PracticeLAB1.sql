CREATE DATABASE Blog
USE Blog
CREATE TABLE Article
(
    Id          INT PRIMARY KEY IDENTITY,
    Title       NVARCHAR(50)  NOT NULL,
    Description NVARCHAR(200) NOT NULL

)
CREATE TABLE Comment
(
    Id   INT PRIMARY KEY IDENTITY,
    Text NVARCHAR(200) NOT NULL
)

CREATE TABLE Authors
(
    Id        INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName  NVARCHAR(50) NOT NULL
)

ALTER TABLE Article
    ADD AuthorId INT FOREIGN KEY REFERENCES Authors (Id)

ALTER TABLE Comment
    ADD ArticleId INT FOREIGN KEY REFERENCES Article (Id)

ALTER TABLE Comment
    ADD AuthorId int FOREIGN KEY REFERENCES Authors (Id)

CREATE TABLE ArticleAuthors
(
    AuthorId  int FOREIGN KEY REFERENCES Authors (Id),
    ArticleId int FOREIGN KEY REFERENCES Article (Id)
)
USE Geography

SELECT *
FROM CountriesRivers
         JOIN Countries C ON CountriesRivers.CountryCode = C.CountryCode

SELECT CountryName, ContinentName
FROM Countries
         JOIN Continents C ON Countries.ContinentCode = C.ContinentCode

USE Blog

SELECT Ar.Title, c.Text, A.FirstName, A.LastName
FROM Comment C
         JOIN Authors A
              ON C.Id = A.Id
         JOIN Article Ar
              ON C.Id = Ar.Id

USE Geography

SELECT MountainRange, PeakName, Elevation
FROM Peaks
         JOIN Mountains M ON M.Id = Peaks.MountainId
WHERE MountainRange = 'Rila'
ORDER BY Elevation DESC

