USE master

CREATE DATABASE Bitbucket

USE Bitbucket

CREATE TABLE Users
(
    Id       int PRIMARY KEY IDENTITY,
    Username varchar(30) NOT NULL,
    Password varchar(30) NOT NULL,
    Email    varchar(50) NOT NULL,

)
CREATE TABLE Repositories
(
    Id   int PRIMARY KEY IDENTITY,
    Name varchar(50) NOT NULL

)
CREATE TABLE RepositoriesContributors
(
    RepositoryId  int NOT NULL REFERENCES Repositories (Id),
    ContributorId int NOT NULL REFERENCES Users (Id)

)

CREATE TABLE Issues
(
    Id           int PRIMARY KEY IDENTITY,
    Title        varchar(max) NOT NULL,
    IssueStatus  char(6)      NOT NULL,
    RepositoryId int          NOT NULL REFERENCES Repositories (Id),
    AssigneeId   int          NOT NULL REFERENCES Users (Id)

)

CREATE TABLE Commits
(
    Id            int PRIMARY KEY IDENTITY,
    Message       varchar(max) NOT NULL,
    IssueId       int REFERENCES Issues (Id),
    RepositoryId  int          NOT NULL REFERENCES Repositories (Id),
    ContributorId int          NOT NULL REFERENCES Users (Id)

)

CREATE TABLE Files
(
    Id       int PRIMARY KEY IDENTITY,
    Name     varchar(100)   NOT NULL,
    Size     decimal(18, 2) NOT NULL,
    ParentId int REFERENCES Files (Id),
    CommitId int            NOT NULL REFERENCES Commits (Id)

)

--2.Insert

INSERT INTO Files(Name, Size, ParentId, CommitId)
VALUES ('Trade.idk', 2598.0, 1, 1),
       ('menu.net', 9238.31, 2, 2),
       ('Administrate.soshy', 1246.93, 3, 3),
       ('Controller.php', 7353.15, 4, 4),
       ('Find.java', 9957.86, 5, 5),
       ('Controller.json', 14034.87, 3, 6),
       ('Operate.xix', 7662.92, 7, 7)

INSERT INTO Issues(Title, IssueStatus, RepositoryId, AssigneeId)
VALUES ('Critical Problem with HomeController.cs file', 'open', 1, 4),
       ('Typo fix in Judge.html', 'open', 4, 3),
       ('Implement documentation for UsersService.cs', 'closed', 8, 2),
       ('Unreachable code in Index.cs', 'open', 9, 8)

--3.	Update

UPDATE Issues
SET IssueStatus='closed'
WHERE AssigneeId = 6

--4.	Delete

DELETE RepositoriesContributors
WHERE RepositoryId = (SELECT id FROM Repositories WHERE Name LIKE 'Softuni-Teamwork')

DELETE Issues
WHERE RepositoryId = (SELECT id FROM Repositories WHERE Name LIKE 'Softuni-Teamwork')


USE master

--5.	Commits
SELECT *
FROM Commits
ORDER BY Id, Message, RepositoryId, ContributorId

--6.	Heavy HTML

SELECT id, Name, Size
FROM Files
WHERE Size > 1000
  AND Name LIKE '%html'
ORDER BY Size DESC, id, Name

--7.	Issues and Users

SELECT i.id, CONCAT(u.Username, ' : ', i.Title) AS IssueAssignee
FROM Issues AS i
         JOIN Bitbucket.dbo.Users AS u ON i.AssigneeId = u.Id
ORDER BY i.id DESC, IssueAssignee;

--8.	Non-Directory Files

SELECT f1.id, f1.Name, CONCAT(f1.size, 'KB') AS Size
FROM Files AS f1
         LEFT JOIN Files AS f2 ON f1.id = f2.ParentId
WHERE f2.ParentId IS NULL
ORDER BY id, Name, Size DESC

--9.	Most Contributed Repositories

SELECT TOP (5) r.Id, r.Name, COUNT(c.Id) AS Commits
FROM Repositories AS r
         JOIN Commits C ON r.Id = C.RepositoryId
         JOIN RepositoriesContributors RC ON r.Id = RC.RepositoryId
GROUP BY r.Id, r.Name
ORDER BY Commits DESC, r.Id, r.Name;

--10.	User and Files

SELECT u.Username, AVG(f.size) AS Size
FROM Bitbucket.dbo.Users AS u
         JOIN Commits AS c ON u.Id = c.ContributorId
         JOIN Files AS f ON c.Id = f.CommitId
GROUP BY u.Username
ORDER BY Size DESC, u.Username;

--Section 4. Programmability
--11.   User Total Commits
USE Bitbucket
CREATE FUNCTION udf_UserTotalCommits(@username varchar(30))
    RETURNS int AS
BEGIN
    RETURN (SELECT COUNT(id)
            FROM Commits
            WHERE ContributorId = (SELECT id FROM Bitbucket.dbo.Users WHERE Username LIKE @username))
END

SELECT dbo.udf_UserTotalCommits('UnderSinduxrein')

--12.	 Find by Extensions

CREATE PROC usp_FindByExtension(@extension varchar(10))
AS
BEGIN
    SELECT id, name, CONCAT(size, 'KB') AS Size
    FROM files
    WHERE name LIKE '%' + @extension
    ORDER BY id, name, Size
END

    EXEC usp_FindByExtension 'txt'





