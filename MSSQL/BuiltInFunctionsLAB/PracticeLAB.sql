USE SoftUni1

SELECT TOP (10) ProjectID,
                Name,
                Description,
                StartDate,
                EndDate,
                DATEPART(WEEK, StartDate)
FROM SoftUni1.dbo.Projects;

SELECT *,
       PERCENTILE_CONT(0.5)
                       WITHIN GROUP ( ORDER BY Salary DESC )
                       OVER (PARTITION BY DepartmentID) AS MedianCont
FROM Employees;

SELECT CONCAT(FirstName, ' ', LastName)
           AS [Full Name]
FROM Employees

SELECT CONCAT_WS(' ', FirstName, LastName)
FROM Employees;

SELECT CONCAT_WS(' ', FirstName, SUBSTRING(LastName, 1, 3)) + '...'
FROM Employees;

SELECT REPLACE(LastName, 'G', 'SSSS')
FROM Employees;

SELECT LTRIM('   Niki   ')
SELECT RTRIM('   Niuki    ')
SELECT TRIM('     niki    ');
SELECT LEN(LTRIM('   Niki   '))
SELECT LEN(TRIM('   Niki   '))
SELECT LEN(RTRIM('   Niki   ')), DATALENGTH('someData')

SELECT TOP (10) FirstName, UPPER(LastName)
FROM Employees

SELECT LOWER(N'нИКИ')

SELECT REVERSE(FirstName), LastName
FROM Employees

SELECT REPLICATE('*', 14)

SELECT FORMAT(GETDATE(), 'dd MMMM yyyy', 'fr-FR')

SELECT FORMAT(0.14, 'P')

SELECT FORMAT(102.65, 'c', 'bg-BG')

SELECT FORMAT(102.65, 'c', 'fr-FR')

SELECT FORMAT(102.65, 'c')

SELECT VALUE,
       LEN(value),
       SUBSTRING(value, 1, 3)
FROM
    STRING_SPLIT('SOME RANDOM TEXT', ' ')

SELECT FORMAT(1.22221515, 'F5')
SELECT FORMAT(1.22221515, 'P5')

SELECT FORMAT(CAST('2010-01-01' AS DATETIME), 'MMMM')

SELECT CHARINDEX('Niki', 'I am Niki Kostov. Yes, Niki', 10)

SELECT STUFF('I am Niki Kostov. Yes, Niki', 6, 5, 'not ')

SELECT 2 + 6

SELECT 2 * 6 + 10

SELECT 1 / 2 /*0*/

SELECT 1.0 / 2

USE demo
SELECT *,
       1.0 * Quantity / BoxCapacity
FROM Products;

SELECT *,
       CAST(Quantity AS float) / BoxCapacity
FROM Products;

SELECT *
FROM Triangles2;

CREATE VIEW v_Triangles2Areas
AS
SELECT id, a, h, (A * H) / 2 AS Area
FROM Triangles2

SELECT 10 % 4

SELECT PI() * 2

SELECT ABS(-200)

SELECT *,
       ABS(x1),
       SQRT(x2),
       SQUARE(x2),
       POWER(x2, 2)
FROM Lines

SELECT x1,
       y1,
       x2,
       y2,
       SQRT(SQUARE(x1 - x2) + SQUARE(y1 - y2))
           AS LengthBetweenTwoPoints
FROM Lines

SELECT ROUND(1.2254645, 2) + 0.1

SELECT ROUND(16559.16546, -2)/*16600.00000*/

SELECT *,
       CEILING
           (1.0 * Quantity / (BoxCapacity * PalletCapacity))
           AS PalletsNeeded,
       BoxCapacity * Products.PalletCapacity
           AS QuantityPerPallet
FROM Products;

SELECT RAND(4)
/*/////////////
  */

USE SoftUni1

SELECT StartDate,
       EndDate,
       DATEPART(YEAR, StartDate),
       YEAR(EndDate),
       DATEPART(MONTH, StartDate),
       DATEPART(QUARTER, EndDate),
       DATEPART(WEEKDAY, StartDate) AS WeekDay,
       DATENAME(WEEKDAY, StartDate) AS NameOfWeekDay,

FROM Projects

SELECT ProjectID,
       StartDate,
       DATEPART(QUARTER, StartDate)       AS Quarter,
       DATEPART(MONTH, StartDate)         AS Month,
       DATEPART(YEAR, StartDate)          AS Year,
       DATEPART(DAY, StartDate)           AS Day,
       DATENAME(WEEKDAY, StartDate)       AS DayOfWeek,
       DATEDIFF(DAY, StartDate, EndDate)  AS DayDiff,
       DATEDIFF(WEEK, StartDate, EndDate) AS WeekDiff,
       EOMONTH(StartDate),
       DATEADD(MONTH, 1, EndDate)

FROM Projects
ORDER BY DayDiff DESC

SELECT StartDate,
       UPPER(FORMAT(StartDate, 'yyyy MMMM dd (dddd)', 'bg-BG'))
FROM Projects

SELECT CAST(1.2 AS int)
SELECT CONVERT(int, 2.5)
SELECT CAST('2020' AS date)

SELECT FirstName, MiddleName, ISNULL(MiddleName, 'No data'), LastName
FROM Employees

SELECT name, ISNULL(CONVERT(VARCHAR, EndDate), 'Not finished') AS FinishDate
FROM Projects

SELECT COALESCE(NULL, 5, 6, NULL, 7)/*TAKES THE FIRST NOT NULL ELEMENT*/

SELECT FirstName, LastName
FROM Employees
ORDER BY FirstName
OFFSET 10 ROWS FETCH NEXT 5 ROWS ONLY

SELECT *
FROM Projects
ORDER BY StartDate
OFFSET 10 ROWS FETCH NEXT 20 ROWS ONLY

SELECT *
FROM (SELECT TOP (100) *,
                       ROW_NUMBER()
                               OVER (ORDER BY Salary DESC)       AS RowNo,
                       RANK()
                               OVER (ORDER BY Salary DESC )      AS Rank,
                       DENSE_RANK()
                               OVER (ORDER BY Salary DESC )      AS DenseRank,
                       NTILE(10) OVER (ORDER BY Salary DESC )    AS GroupNo,
                       SUM(Salary) OVER (ORDER BY Salary DESC )  AS SalarySum,
                       AVG(Salary) OVER ( ORDER BY Salary DESC ) AS ArgSalary,
                       MIN(Salary) OVER ( ORDER BY DepartmentID) AS MinSalaryByDepart
      FROM Employees
WHERE JobTitle LIKE '[^EF]%manage_'
      ORDER BY Salary DESC) AS TempResult
WHERE GroupNo = 2

SELECT *
FROM Employees
WHERE JobTitle LIKE '[^S-Z]%'


