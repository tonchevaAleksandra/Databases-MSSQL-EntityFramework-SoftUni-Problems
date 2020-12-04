CREATE DATABASE SoftUni

USE SoftUni

CREATE TABLE Towns (
	Id INT PRIMARY KEY IDENTITY, 
	Name NVARCHAR(20) NOT NULL
	)

CREATE TABLE Addresses (
	Id INT PRIMARY KEY IDENTITY, 
	AddressText NVARCHAR(100) NOT NULL, 
	TownId INT NOT NULL

	CONSTRAINT FK_Addresses_TownId
		FOREIGN KEY (TownId)
			REFERENCES Towns (Id)
	)

CREATE TABLE Departments (
	Id INT PRIMARY KEY IDENTITY, 
	Name NVARCHAR(50) NOT NULL
	)

CREATE TABLE Employees (
	Id INT IDENTITY PRIMARY KEY,
	FirstName NVARCHAR(20) NOT NULL,
	MiddleName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	JobTitle NVARCHAR(20) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments (Id),
	HireDate DATE NOT NULL,
	Salary DECIMAL(7, 2) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses (Id)
	)




