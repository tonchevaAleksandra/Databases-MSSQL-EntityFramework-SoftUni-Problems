USE master

CREATE DATABASE Service

CREATE TABLE Users
(
    Id        int PRIMARY KEY IDENTITY,
    Username  nvarchar(30) UNIQUE NOT NULL,
    Password  nvarchar(50)        NOT NULL,
    [Name]    nvarchar(50),
    Birthdate datetime2,
    Age       int CHECK (Age BETWEEN 14 AND 110),
    Email     nvarchar(30)        NOT NULL

)

CREATE TABLE Departments
(
    Id   int PRIMARY KEY IDENTITY,
    Name nvarchar(50) NOT NULL
)

CREATE TABLE Employees
(
    Id           int PRIMARY KEY IDENTITY,
    FirstName    nvarchar(25),
    LastName     nvarchar(25),
    Birthdate    datetime2,
    Age          int CHECK (Age BETWEEN 18 AND 110),
    DepartmentId int REFERENCES Departments (Id)

)

CREATE TABLE Categories
(
    Id           int PRIMARY KEY IDENTITY,
    Name         nvarchar(50) NOT NULL,
    DepartmentId int          NOT NULL REFERENCES Departments (Id)

)

CREATE TABLE Status
(
    Id    int PRIMARY KEY IDENTITY,
    Label nvarchar(30) NOT NULL

)

CREATE TABLE Reports
(
    Id          int PRIMARY KEY IDENTITY,
    CategoryId  int           NOT NULL REFERENCES Categories (Id),
    StatusId    int           NOT NULL REFERENCES Status (Id),
    OpenDate    datetime2     NOT NULL,
    CloseDate   datetime2,
    Description nvarchar(200) NOT NULL,
    UserId      int           NOT NULL REFERENCES Users (Id),
    EmployeeId  int REFERENCES Employees (Id)

)

--Section 2 DML

INSERT INTO Employees(FirstName, LastName, Birthdate, Age, DepartmentId)
VALUES ('Marlo', 'O''Malley', '1958-09-21', DATEDIFF(YEAR, Birthdate, GETDATE()), 1,
        'Niki', 'Stanaghan', '1969-11-26', DATEDIFF(YEAR, Birthdate, GETDATE()), 4,
        'Ayrton', 'Senna', '1960-03-21', DATEDIFF(YEAR, Birthdate, GETDATE()), 9,
        'Ronnie', 'Peterson', '1944-02-14', DATEDIFF(YEAR, Birthdate, GETDATE()), 9,
        'Giovanna', 'Amati', '1959-07-20', DATEDIFF(YEAR, Birthdate, GETDATE()), 5)
GO




