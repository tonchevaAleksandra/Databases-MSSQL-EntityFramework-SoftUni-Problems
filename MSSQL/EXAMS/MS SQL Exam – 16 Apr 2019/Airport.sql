USE master

CREATE DATABASE Airport

USE Airport

CREATE TABLE Planes
(
    Id    int PRIMARY KEY IDENTITY,
    Name  varchar(30) NOT NULL,
    Seats int         NOT NULL,
    Range int         NOT NULL
)

CREATE TABLE Flights
(
    Id            int PRIMARY KEY IDENTITY,
    DepartureTime datetime,
    ArrivalTime   datetime,
    Origin        varchar(50) NOT NULL,
    Destination   varchar(50) NOT NULL,
    PlaneId       int         NOT NULL REFERENCES Planes (Id)
)

CREATE TABLE Passengers
(
    Id         int PRIMARY KEY IDENTITY,
    FirstName  varchar(30) NOT NULL,
    LastName   varchar(30) NOT NULL,
    Age        int         NOT NULL,
    Address    varchar(30) NOT NULL,
    PassportId char(11)    NOT NULL
)

CREATE TABLE LuggageTypes
(
    Id   int PRIMARY KEY IDENTITY,
    Type varchar(30) NOT NULL
)

CREATE TABLE Luggages
(
    Id            int PRIMARY KEY IDENTITY,
    LuggageTypeId int NOT NULL REFERENCES LuggageTypes (Id),
    PassengerId   int NOT NULL REFERENCES Passengers (Id)
)

CREATE TABLE Tickets
(
    Id          int PRIMARY KEY IDENTITY,
    PassengerId int            NOT NULL REFERENCES Passengers (Id),
    FlightId    int            NOT NULL REFERENCES Flights (Id),
    LuggageId   int            NOT NULL REFERENCES Luggages (Id),
    Price       decimal(15, 2) NOT NULL
)


INSERT INTO Planes(Name, Seats, Range)
VALUES ('Airbus 336', 112, 5132),
       ('Airbus 330', 432, 5325),
       ('Boeing 369', 231, 2355),
       ('Stelt 297', 254, 2143),
       ('Boeing 338', 165, 5111),
       ('Airbus 558', 387, 1342),
       ('Boeing 128', 345, 5541)
GO

INSERT INTO LuggageTypes(Type)
VALUES ('Crossbody Bag'),
       ('School Backpack'),
       ('Shoulder Bag')


--3.Update
/*UPDATE Tickets
SET Price+= Price * 0.13
WHERE id IN (
    SELECT t.id
    FROM Tickets AS t
             JOIN Flights F ON F.Id = t.FlightId
    WHERE f.Destination = 'Carlsbad')*/

UPDATE Tickets
SET Price+=Price * 0.13
WHERE FlightId IN (SELECT id
                   FROM Flights
                   WHERE Destination = 'Carlsbad')

DELETE Tickets
WHERE FlightId IN
      (SELECT id
       FROM Flights
       WHERE Destination = 'Ayn Halagim');

DELETE Flights
WHERE Destination = 'Ayn Halagim';

USE master

SELECT Origin, Destination
FROM Flights
ORDER BY Origin, Destination

SELECT *
FROM Planes
WHERE Name LIKE '%tr%'
ORDER BY id, Name, Seats, Range


SELECT f.Id as FlightId,
       Sum(Price) as Price
FROM Flights as f
JOIN Tickets T ON f.Id = T.FlightId
GROUP BY f.Id
ORDER BY Price desc, f.Id

SELECT p.FirstName,p.LastName, sum(t.Price) as Price
FROM Passengers as p
JOIN Tickets T ON p.Id = T.PassengerId
GROUP BY p.FirstName, p.LastName
ORDER BY Price desc, p.FirstName,p.LastName





