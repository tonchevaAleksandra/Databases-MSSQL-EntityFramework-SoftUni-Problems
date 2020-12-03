CREATE DATABASE Hotel

USE Hotel

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(15) NOT NULL,
	LastName NVARCHAR(15) NOT NULL, 
	Title NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(200) 
	)

INSERT INTO Employees (FirstName, LastName, Title, Notes)
	VALUES
		('IVAN', 'KOSEV', 'RECEPTIONIST',NULL),
		('VANYA', 'STAMATOVA', 'MANAGER',NULL),
		('SANDRA','MAKSIMOVA', 'MAID', NULL)

CREATE TABLE Customers (
	AccountNumber NVARCHAR(15) PRIMARY KEY ,
	FirstName NVARCHAR(15) NOT NULL, 
	LastName NVARCHAR(15) NOT NULL,
	PhoneNumber NVARCHAR(15) NOT NULL, 
	EmergencyName NVARCHAR(50) NOT NULL, 
	EmergencyNumber NVARCHAR(15) NOT NULL,
	Notes NVARCHAR(200)
)

INSERT INTO Customers (AccountNumber, FirstName, LastName, 
PhoneNumber, EmergencyName, EmergencyNumber,
Notes)
	VALUES
		('1','SIMO','ANDONOV','8851264','1','123', NULL),
		('2','SIMO','ANDONOV','8851264','1','123', NULL),
		('3','SIMO','ANDONOV','8851264','1','123', NULL)


CREATE TABLE RoomStatus (
	RoomStatus NVARCHAR(20) PRIMARY KEY, 
	Notes NVARCHAR(100)
	)

INSERT INTO RoomStatus (RoomStatus, Notes)
	VALUES
		('FREE AND CLEAN', NULL),
		('FREE, NOT CLEAN', NULL),
		('OCCUPIED', NULL)

CREATE TABLE RoomTypes (
	RoomType NVARCHAR(15) PRIMARY KEY,
	Notes NVARCHAR(100) 
	)

INSERT INTO RoomTypes (RoomType, Notes)
	VALUES
		('FANCY',NULL),
		('FAMILLY', NULL),
		('SINGLE', NULL)

CREATE TABLE BedTypes (
	BedType NVARCHAR(10) PRIMARY KEY,
	Notes NVARCHAR(100) 
	)

INSERT INTO BedTypes (BedType, Notes)
	VALUES
		('ONEPERSON', NULL),
		('TWOPERSONS', NULL),
		('KINGBED', NULL)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY IDENTITY, 
	RoomType NVARCHAR(15) FOREIGN KEY REFERENCES 
	RoomTypes(RoomType), 
	BedType NVARCHAR(10) FOREIGN KEY REFERENCES 
	BedTypes(BedType), 
	Rate DECIMAL(4,2), 
	RoomStatus NVARCHAR(20) FOREIGN KEY REFERENCES
	RoomStatus(RoomStatus), 
	Notes NVARCHAR(200)
	)

INSERT INTO Rooms (RoomType, BedType, Rate, RoomStatus, Notes)
	VALUES
		('FANCY','KINGBED', 85,'OCCUPIED',NULL),
		('FAMILLY','TWOPERSONS', 65,'FREE, NOT CLEAN',NULL),
		('SINGLE','ONEPERSON', 35,'FREE AND CLEAN',NULL)

CREATE TABLE Payments (
	Id INT PRIMARY KEY IDENTITY, 
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id), 
	PaymentDate DATETIME2 NOT NULL, 
	AccountNumber NVARCHAR(15) FOREIGN KEY REFERENCES 
	Customers(AccountNumber), 
	FirstDateOccupied DATETIME2 NOT NULL, 
	LastDateOccupied DATETIME2 NOT NULL,
	TotalDays AS DATEDIFF(DAY,FirstDateOccupied, LastDateOccupied),
	AmountCharged DECIMAL(6,2) NOT NULL,
	TaxRate DECIMAL(4,2) NOT NULL,
	TaxAmount AS AmountCharged*TaxRate, 
	PaymentTotal DECIMAL(6,2) NOT NULL, 
	Notes NVARCHAR(200)
	)

INSERT INTO Payments (EmployeeId, PaymentDate, AccountNumber,
FirstDateOccupied, LastDateOccupied,
AmountCharged, TaxRate,
PaymentTotal, Notes)
	VALUES
		('1','2020-05-20','1','2020-05-20','2020-05-25',220,20,
		260,NULL),
		('2','2020-05-20','2','2020-05-20','2020-05-25',220,20,
		260,NULL),
		('3','2020-05-20','3','2020-05-20','2020-05-25',220,20,
		260,NULL)

CREATE TABLE Occupancies (
	Id INT PRIMARY KEY IDENTITY, 
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id), 
	DateOccupied DATETIME2 , 
	AccountNumber NVARCHAR(15) FOREIGN KEY REFERENCES 
	Customers(AccountNumber), 
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber), 
	RateApplied DECIMAL(4,2), 
	PhoneCharge DECIMAL(5,2),
	Notes NVARCHAR(200)
	)

INSERT INTO Occupancies (EmployeeId, DateOccupied,
AccountNumber, RoomNumber, RateApplied, PhoneCharge,
Notes)
	VALUES
		(1,'2020-05-20','1',1,25.5, 30, NULL),
		(2,'2020-05-20','2',2,25.5, 30, NULL),
		(3,'2020-05-20','3',3,25.5, 30, NULL)

