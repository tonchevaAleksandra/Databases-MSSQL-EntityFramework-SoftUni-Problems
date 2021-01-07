USE master
CREATE DATABASE MinionsDB
USE MinionsDB

CREATE TABLE Countries
(
    Id   int PRIMARY KEY IDENTITY,
    Name nvarchar(30) NOT NULL
)

CREATE TABLE Towns
(
    Id          int PRIMARY KEY IDENTITY,
    Name        nvarchar(50) NOT NULL,
    CountryCode int          NOT NULL REFERENCES Countries (Id)
)

CREATE TABLE Minions
(
    Id     int PRIMARY KEY IDENTITY,
    Name   nvarchar(50) NOT NULL,
    Age    int          NOT NULL CHECK (Age >= 0),
    TownId int          NOT NULL REFERENCES Towns (Id)
)

CREATE TABLE EvilnessFactors
(
    Id   int PRIMARY KEY IDENTITY,
    Name nvarchar(50) NOT NULL
        CHECK (Name IN ('super good', 'good', 'bad', 'evil', 'super evil'))
)

CREATE TABLE Villains
(
    Id               int PRIMARY KEY IDENTITY,
    Name             nvarchar(30) NOT NULL,
    EvilnessFactorId int          NOT NULL REFERENCES EvilnessFactors (Id)
)

CREATE TABLE MinionsVillains
(
    MinionId  int NOT NULL REFERENCES Minions (Id),
    VillainId int NOT NULL REFERENCES Villains (Id),
    PRIMARY KEY (MinionId, VillainId)
)
use master
drop DATABASE MinionsDB