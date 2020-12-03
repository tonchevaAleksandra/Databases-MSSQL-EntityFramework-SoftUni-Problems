CREATE DATABASE CarRental

USE CarRental

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL(4,2),
	WeeklyRate DECIMAL(4,2),
	MonthlyRate DECIMAL(4,2),
	WeekendRate DECIMAL(4,2)
	)

CREATE TABLE Cars(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber NVARCHAR(10) NOT NULL,
	Manufacturer NVARCHAR(20) NOT NULL,
	Model NVARCHAR(20) NOT NULL,
	CarYear DATETIME2 NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors INT ,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(20),
	Available BIT NOT NULL
	)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(15) NOT NULL,
	LastName NVARCHAR(15) NOT NULL,
	Title NVARCHAR(15) ,
	Notes NVARCHAR(100) 
	)

CREATE TABLE Customers(
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber NVARCHAR(20) NOT NULL,
	FullName NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(50),
	City NVARCHAR(15) NOT NULL,
	ZIPCode INT NOT NULL,
	Notes NVARCHAR(100)
	)

CREATE TABLE RentalOrders(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel INT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage AS KilometrageEnd-KilometrageStart,
	StartDate DATETIME2 NOT NULL,
	EndDate DATETIME2 NOT NULL,
	TotalDays AS DATEDIFF(DAY,StartDate,EndDate),
	RateApplied DECIMAL(5,2) ,
	TaxRate DECIMAL(7,2) NOT NULL,
	OrderStatus NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(200) 
	)

INSERT INTO Categories(CategoryName,DailyRate,
WeeklyRate,MonthlyRate,WeekendRate)
	VALUES
		('SUV', 25.30,15.20,13.50,40.00),
		('SEDAN', 12.30,15.20,13.50,10.00),
		('SPORT', 21.20,13.00,16.00,15.00)
		
INSERT INTO Cars(PlateNumber, Manufacturer, Model,
CarYear,CategoryId,Doors,Picture, Condition,Available)
	VALUES
		('1','BMW', 'X5', '2018', 1, 5,NULL, 'NEW',0),
		('2','AUDI', 'A5', '2018', 2, 5,NULL, 'NEW',0),
		('3','AUDI', 'R8', '2019', 3, 5,NULL, 'NEW',0)

INSERT INTO Employees(FirstName,LastName,Title,Notes)
	VALUES
		('IVAN','CHAPKANOV','MOSS', NULL),
		('STOQN','PETROV','BOSS', NULL),
		('PARVAN','SEMOV','BARBADOS', NULL)

INSERT INTO Customers(DriverLicenceNumber,FullName,
[Address],City,ZIPCode,Notes)
	VALUES
		('3166','STAMAT HRISTOV', NULL,'SOFIA',1300,NULL),
		('4528','STEFAN HRISTOV', NULL,'VARNA',9000,NULL),
		('2189','VILI HRISTOV', NULL,'PLEVEN',5800,NULL)

INSERT INTO RentalOrders(EmployeeId,CustomerId,CarId,
TankLevel,KilometrageStart,KilometrageEnd,
StartDate,EndDate,
RateApplied,TaxRate,OrderStatus,Notes)
	 VALUES
		(1,1,1,5,20,40,'2020-05-16','2020-05-20',
		NULL,65.00,'COMPLETE', NULL),
		(2,2,2,5,20,50,'2020-05-16','2020-05-20',
		NULL,65.00,'COMPLETE', NULL),
		(3,3,3,5,30,100,'2020-05-16','2020-05-20',
		NULL,65.00,'COMPLETE', NULL)