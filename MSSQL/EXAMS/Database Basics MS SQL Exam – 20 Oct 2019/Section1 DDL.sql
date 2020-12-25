USE master

CREATE DATABASE Service

CREATE TABLE Users
(
    Id        int PRIMARY KEY IDENTITY,
    Username  nvarchar(30) UNIQUE NOT NULL,
    Password  nvarchar(50)        NOT NULL,
    [Name]    nvarchar(50),
    Birthdate datetime,
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
    Id           INT PRIMARY KEY IDENTITY,
    FirstName    NVARCHAR(25),
    LastName     NVARCHAR(25),
    Birthdate    DATETIME,
    Age          INT CHECK (Age BETWEEN 18 AND 110),
    DepartmentId INT REFERENCES Departments (Id)

)
GO



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

INSERT INTO Employees(firstname, lastname, birthdate, departmentid)
VALUES ('Marlo', 'O''Malley', '1958-09-21', 1),
       ('Niki', 'Stanaghan', '1969-11-26', 4),
       ('Ayrton', 'Senna', '1960-03-21', 9),
       ('Ronnie', 'Peterson', '1944-02-14', 9),
       ('Giovanna', 'Amati', '1959-07-20', 5)

GO
SET IDENTITY_INSERT [dbo].Reports OFF;
INSERT INTO Reports(categoryid, statusid, opendate, closedate, description, userid, employeeid)
VALUES (1, 1, '2017-04-13', NULL, 'Stuck Road on Str.133', 6, 2),
       (6, 3, '2015-09-05', '2015-12-06', 'Charity trail running', 3, 5),
       (14, 2, '2015-09-07', NULL, 'Falling bricks on Str.58', 5, 2),
       (4, 3, '2017-07-03', '2017-07-06', 'Cut off streetlight on Str.11', 1, 1)


UPDATE Reports
SET CloseDate=GETDATE()
WHERE CloseDate IS NULL

DELETE Reports
WHERE StatusId = 4

--Section 3

SELECT Description,
       (SELECT FORMAT(OpenDate, 'dd-MM-yyyy')) AS [OpenDate]
FROM Reports
WHERE EmployeeId IS NULL
ORDER BY OpenDate, Description


SELECT Description
FROM Reports
WHERE CategoryId IS NOT NULL
ORDER BY Description, CategoryId

SELECT TOP (5) c.Name AS CategoryName, COUNT(r.Id) AS ReportsNumber
FROM Categories AS c
         JOIN Reports AS r ON c.Id = r.CategoryId
GROUP BY c.Name
ORDER BY ReportsNumber DESC, c.Name

SELECT u.Username AS Username, c.Name AS CategoryName
FROM Reports AS r
         JOIN Users AS u ON r.UserId = u.Id
         JOIN Categories C ON C.Id = r.CategoryId
WHERE DAY(u.Birthdate) = DAY(r.OpenDate)
  AND MONTH(u.Birthdate) = MONTH(r.OpenDate)
ORDER BY u.Username, CategoryName

SELECT CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
       COUNT(u.id)                          AS UsersCount
FROM Employees AS e
         JOIN Reports R2 ON e.Id = R2.EmployeeId
         JOIN Users U ON U.Id = R2.UserId
GROUP BY e.FirstName, e.LastName
ORDER BY UsersCount DESC, FullName


SELECT ISNULL(CONCAT(E.FirstName, ' ', E.LastName), 'None') AS Employee,
       ISNULL(D.Name, 'None')                               AS Department,
       ISNULL(C.Name, 'None')                               AS Category,
       ISNULL(r.Description, 'None')                        AS [Description],
       ISNULL(FORMAT(r.OpenDate, 'dd.MM.yyyy'), 'None')     AS OpenDate,
       ISNULL(S.Label, 'None')                              AS [Status],
       ISNULL(u.Name, 'None')                               AS [User]
FROM Reports AS R
         LEFT JOIN Service.dbo.Employees E
                   ON E.Id = R.EmployeeId
         LEFT JOIN Service.dbo.Departments D ON E.DepartmentId = D.Id
         LEFT JOIN Service.dbo.Categories C ON C.Id = R.CategoryId
         LEFT JOIN Service.dbo.Users U ON U.Id = R.UserId
         LEFT JOIN Service.dbo.Status S ON S.Id = R.StatusId
ORDER BY e.FirstName DESC,
         e.LastName DESC,
         d.Name,
         c.Name,
         r.Description,
         r.OpenDate,
         s.Label,
         u.Name;

--Section 4 Programmability

CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
    RETURNS INT
AS
BEGIN
    IF @StartDate IS NULL OR @EndDate IS NULL
        BEGIN
            RETURN 0;
        END
    DECLARE @Hours int = DATEDIFF(HOUR, @StartDate, @EndDate)
    RETURN @Hours
END

SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
FROM Reports

CREATE OR
ALTER PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
    DECLARE @DepartmentEmployee int =(SELECT DepartmentId
                                      FROM Service.dbo.Employees AS e
                                      WHERE e.Id = @EmployeeId);
    DECLARE @DepartmentReport int=
        (SELECT c.DepartmentId
         FROM Service.dbo.Reports AS r
                  JOIN Service.dbo.Categories C ON C.Id = r.CategoryId
         WHERE r.Id = @ReportId
        );
    IF (@DepartmentEmployee <> @DepartmentReport)
        BEGIN
            THROW 50001,'Employee doesn''t belong to the appropriate department!',1;

        END

    UPDATE Service.dbo.Reports
    SET Service.dbo.Reports.EmployeeId=@EmployeeId
    WHERE Service.dbo.Reports.Id = @ReportId
END
GO


EXEC usp_AssignEmployeeToReport 30, 1
EXEC usp_AssignEmployeeToReport 17, 2







