USE SoftUni1

SELECT TownId, [Name], LEN(Name), SUBSTRING(Name, 1, 3)
FROM Towns
WHERE LEFT(Name, 1) = 'S'

SELECT TownId,
       [Name],
       LEN([Name])             AS NameLength,
       SUBSTRING([Name], 1, 3) AS FirstThreeLetters
FROM Towns
WHERE LEFT(Name, 1) = 'S'

SELECT GETDATE()

SELECT AddressText, [Name]
FROM Addresses
         JOIN Towns ON Addresses.TownID = Towns.TownID
WHERE Name = 'Bellevue'

SELECT A.AddressText, T.Name
FROM Addresses A
         JOIN Towns T ON A.TownID = T.TownID
WHERE Name = 'Bellevue'

SELECT *
FROM (SELECT TownId,
             [Name],
             LEN([Name])             AS NameLength,
             SUBSTRING([Name], 1, 3) AS FirstThreeLetters
      FROM Towns) AS tmp
WHERE FirstThreeLetters = 'SOF'


SELECT [FirstName] + ' ' + [LastName] AS FullName,
       EmployeeID                     AS [No.]
FROM [Employees]

SELECT TOP (10) [FirstName] + ' ' + [LastName] AS FullName,
                JobTitle,
                Salary
FROM Employees
ORDER BY Salary DESC


SELECT DISTINCT TOP (10) JobTitle, EmployeeID
FROM [Employees]

SELECT COUNT(DISTINCT JobTitle)
FROM Employees

SELECT JobTitle, COUNT(*) AS Count
FROM [Employees]
GROUP BY JobTitle

SELECT JobTitle,
       MAX(Salary)               AS MaxSalary,
       MIN(Salary)               AS MinSalary,
       MAX(Salary) - MIN(Salary) AS DiffSalary
FROM [Employees]
GROUP BY JobTitle
ORDER BY MaxSalary DESC

SELECT *
FROM Employees
WHERE (FirstName LIKE '[A-P]%')
   OR NOT (LastName = 'Nakov')
    AND Salary >= 4000 AND Salary BETWEEN 4000 AND 20000
ORDER BY Salary DESC

SELECT *
FROM Employees
WHERE Salary IN (10000, 2000, 200000, 30000)

SELECT *
FROM Employees
WHERE DepartmentID IN (3, 6);

SELECT *
FROM Employees
WHERE MiddleName IS NOT NULL
  AND MiddleName != ' ';

SELECT *
FROM Employees
ORDER BY FirstName, Salary DESC;

SELECT *
FROM Employees
WHERE YEAR(HireDate) = 2000
  AND DATEPART(DW, HireDate) = 2
--monday=2
--HireDate>='2000-01-01' AND HireDate<'2001-01-01'

CREATE VIEW v_GetHireDateAndDayOfWeek AS
SELECT HireDate, LEFT(DATENAME(DW, HireDate), 3) AS DayOfWeek
FROM Employees

SELECT * FROM v_GetHireDateAndDayOfWeek
ORDER BY DayOfWeek DESC







