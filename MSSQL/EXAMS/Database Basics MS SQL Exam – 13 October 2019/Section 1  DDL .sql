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
    RepositoryId int NOT NULL REFERENCES Repositories(Id),
ContributorId int NOT NULL REFERENCES Users(Id)

)