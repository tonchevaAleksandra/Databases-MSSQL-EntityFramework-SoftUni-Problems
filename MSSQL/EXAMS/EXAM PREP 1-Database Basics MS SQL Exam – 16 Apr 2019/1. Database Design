--create database Airport
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