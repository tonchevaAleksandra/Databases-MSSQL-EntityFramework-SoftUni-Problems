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
         LEFT JOIN Luggages L ON T.LuggageId = L.Id
         LEFT JOIN LuggageTypes LT ON L.LuggageTypeId = LT.Id
WHERE T.PassengerId IS NOT NULL
  AND T.FlightId IS NOT NULL
ORDER BY [Full Name],
         [Plane Name],
         F.Origin,
         F.Destination,
         [Luggage Type]

GO

SELECT RankingQuery.FirstName   AS [First Name],
       RankingQuery.LastName    AS [Last Name],
       RankingQuery.Destination AS Destination,
       RankingQuery.Price
FROM (
         SELECT P.FirstName,
                P.LastName,
                F.Destination,
                Price,
                DENSE_RANK() OVER
                    (PARTITION BY P.FirstName,P.LastName ORDER BY T.Price DESC) AS Rank
         FROM Tickets AS T
                  JOIN Passengers P ON P.Id = T.PassengerId
                  JOIN Flights F ON T.FlightId = F.Id) AS RankingQuery
WHERE RankingQuery.Rank = 1
ORDER BY RankingQuery.Price DESC, RankingQuery.FirstName, RankingQuery.LastName, RankingQuery.Destination
GO


SELECT Destination, COUNT(*) AS FilesCount
FROM Flights
         JOIN Tickets T ON Flights.Id = T.FlightId
GROUP BY Destination
ORDER BY FilesCount DESC, Destination
GO


SELECT p.Name, p.Seats, COUNT(T.id) AS [Passengers Count]
FROM Planes AS p
         JOIN Flights F ON p.Id = F.PlaneId
         JOIN Tickets T ON F.Id = T.FlightId
GROUP BY p.Name, p.Seats
ORDER BY [Passengers Count] DESC, p.Name, p.Seats
GO



CREATE OR
ALTER FUNCTION udf_CalculateTickets(@origin varchar(50),
                                    @destination varchar(50),
                                    @peopleCount int)
    RETURNS varchar(200) AS
BEGIN
    IF (@peopleCount <= 0)
        BEGIN
            RETURN 'Invalid people count!';
        END

    IF (SELECT COUNT(*)
        FROM Flights
        WHERE Origin = @origin
          AND Destination = @destination) < 1
        BEGIN

            RETURN 'Invalid flight!'

        END

    DECLARE @TotalPrice decimal(18, 2)=(SELECT SUM(price)
                                        FROM Tickets
                                                 JOIN Flights F ON F.Id = Tickets.FlightId
                                        WHERE Origin = @origin
                                          AND Destination = @destination) * @peopleCount
    RETURN 'Total price ' + CAST(@TotalPrice AS varchar(30))
END

SELECT dbo.udf_CalculateTickets('Kolyshley', 'Rancabolang', 33)

GO

SELECT dbo.udf_CalculateTickets('Kolyshley', 'Rancabolang', -1)
SELECT dbo.udf_CalculateTickets('Invalid', 'Rancabolang', 33)

GO


CREATE PROC usp_CancelFlights
AS
BEGIN


    UPDATE Flights
    SET ArrivalTime= NULL,
        DepartureTime=NULL
    WHERE ArrivalTime >= Flights.DepartureTime

END
GO

EXEC usp_CancelFlights
/*
SELECT *
from Flights
WHERE ArrivalTime is NULL and DepartureTime is NULL
GO
*/

CREATE TABLE DeletedPlanes
(
    Id    int PRIMARY KEY IDENTITY,
    Name  varchar(30) NOT NULL,
    Seats int         NOT NULL,
    Range int         NOT NULL
)

CREATE TRIGGER tr_InsertPlaneAfterDelete
    ON Planes
    AFTER DELETE AS
BEGIN
    INSERT INTO DeletedPlanes
    SELECT d.Name, d.Seats, d.Range
    FROM deleted AS d
END
GO

DELETE Tickets
WHERE FlightId IN (SELECT Id FROM Flights WHERE PlaneId = 8)

DELETE
FROM Flights
WHERE PlaneId = 8

DELETE
FROM Planes
WHERE Id = 8

SELECT *
FROM DeletedPlanes

