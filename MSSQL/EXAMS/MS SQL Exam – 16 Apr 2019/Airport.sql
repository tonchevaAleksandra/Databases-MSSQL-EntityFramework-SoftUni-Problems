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


SELECT f.Id       AS FlightId,
       SUM(Price) AS Price
FROM Flights AS f
         JOIN Tickets T ON f.Id = T.FlightId
GROUP BY f.Id
ORDER BY Price DESC, f.Id

SELECT p.FirstName, p.LastName, SUM(t.Price) AS Price
FROM Passengers AS p
         JOIN Tickets T ON p.Id = T.PassengerId
GROUP BY p.FirstName, p.LastName
ORDER BY Price DESC, p.FirstName, p.LastName

SELECT FirstName,
       LastName,
       (SELECT SUM(Price)
        FROM Tickets AS t
        WHERE p.Id = t.PassengerId
       ) AS Price
FROM Passengers AS p
ORDER BY Price DESC, FirstName, LastName


SELECT lt.Type, COUNT(*) AS MostUsedLuggage
FROM Luggages AS l
         JOIN LuggageTypes LT ON LT.Id = l.LuggageTypeId
GROUP BY lt.Type
ORDER BY MostUsedLuggage DESC, lt.Type


SELECT CONCAT(p.FirstName, ' ', p.LastName) AS [Full Name], F.Origin, F.Destination
FROM Passengers AS p
         JOIN Tickets T ON p.Id = T.PassengerId
         JOIN Flights F ON T.FlightId = F.Id
ORDER BY [Full Name], F.Origin, F.Destination

SELECT P.FirstName, P.LastName, P.Age
FROM Passengers AS P
         LEFT JOIN Tickets T ON P.Id = T.PassengerId
WHERE T.PassengerId IS NULL
ORDER BY P.Age DESC, P.FirstName, P.LastName


SELECT p.PassportId, p.Address
FROM Passengers AS p
         LEFT JOIN Luggages L ON p.Id = L.PassengerId
WHERE L.PassengerId IS NULL
ORDER BY p.PassportId, p.Address

SELECT p.FirstName AS [First Name],
       p.LastName  AS [Last Name],
       COUNT(*)    AS [Total Trips]
FROM Passengers AS p
         JOIN Tickets T ON p.Id = T.PassengerId
GROUP BY p.FirstName, p.LastName
ORDER BY [Total Trips] DESC, FirstName, LastName


SELECT CONCAT(p.FirstName, ' ', p.LastName)   AS [Full Name],
       P2.Name                                AS [Plane Name],
       CONCAT(F.Origin, ' - ', F.Destination) AS Trip,
       LT.Type                                AS [Luggage Type]
FROM Passengers AS p
         LEFT JOIN Tickets T ON p.Id = T.PassengerId
         LEFT JOIN Flights F ON F.Id = T.FlightId
         LEFT JOIN Planes P2 ON P2.Id = F.PlaneId
         LEFT JOIN Luggages L ON p.Id = L.PassengerId
         LEFT JOIN LuggageTypes LT ON L.LuggageTypeId = LT.Id
WHERE T.PassengerId is not null and T.FlightId is NOT  null
ORDER BY [Full Name],
         [Plane Name],
         F.Origin,
         F.Destination,
         [Luggage Type]

GO



