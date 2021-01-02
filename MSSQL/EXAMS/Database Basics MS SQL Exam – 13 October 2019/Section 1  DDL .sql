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

