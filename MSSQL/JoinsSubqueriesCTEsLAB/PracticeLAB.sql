USE SoftUni1

SELECT A.AddressText, T.Name
FROM Addresses AS A
         JOIN Towns T ON A.TownID = T.TownID

SELECT *
FROM Addresses;

SELECT *
FROM Addresses AS a
         LEFT JOIN Towns T ON a.TownID = T.TownID;
/*even when id is null take the left table and join the other*/

SELECT *
FROM Addresses AS a
         RIGHT JOIN Towns T ON T.TownID = a.TownID;

SELECT *
FROM Addresses AS a
         FULL OUTER JOIN Towns T ON T.TownID = a.TownID/*left+right*/

SELECT *
FROM Addresses
         CROSS JOIN Towns T/*Cartesian product*/

SELECT *
FROM Employees AS e
         JOIN Addresses A ON e.AddressID = A.AddressID
         JOIN Towns T ON T.TownID = A.TownID;

SELECT TOP (50) e.FirstName, e.LastName, T.Name, AddressText
FROM Employees AS e
         JOIN Addresses A ON e.AddressID = A.AddressID
         JOIN Towns T ON T.TownID = A.TownID
ORDER BY FirstName, LastName

SELECT e.EmployeeID, FirstName, LastName, d.Name
FROM Employees AS e
         JOIN Departments d ON d.DepartmentID = e.DepartmentID
WHERE Name LIKE 'Sales'
ORDER BY EmployeeID

SELECT FirstName, LastName, HireDate, Name AS 'DeptName'
FROM Employees AS e
         JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE HireDate > '1999-01-01'
  AND Name IN ('Sales', 'Finance')
ORDER BY HireDate

SELECT TOP (50) e.EmployeeID,
                CONCAT(e.FirstName, ' ', e.LastName)
                       AS 'EmployeeName',
                CONCAT(m.FirstName, ' ', m.LastName)
                       AS 'ManagerName',
                d.Name AS DepartmentName
FROM Employees AS e
         LEFT JOIN Employees AS m ON e.ManagerID = m.EmployeeID
         LEFT JOIN Departments D ON e.EmployeeID = D.ManagerID
ORDER BY e.EmployeeID

SELECT *
FROM Employees AS e
WHERE e.DepartmentID IN
      (
          SELECT DepartmentID
          FROM Departments
          WHERE Name = 'Finance'
      )

SELECT TOP (1) (SELECT AVG(Salary)
                FROM Employees e
                WHERE e.DepartmentID = d.DepartmentID
               ) AS MinAverageSalary
FROM Departments d
WHERE (SELECT COUNT(*)
       FROM Employees e
       WHERE e.DepartmentID = d.DepartmentID) > 0
ORDER BY MinAverageSalary

SELECT DepartmentID, COUNT(*), MIN(Salary), MIN(FirstName)
FROM Employees
GROUP BY DepartmentID

SELECT DepartmentID,
       COUNT(*)                   AS CountEmployees,
       STRING_AGG(FirstName, ' ') AS EmployyesInDepartment
FROM Employees
GROUP BY DepartmentID

SELECT TOP (1) AVG(Salary) AS MinAverageSalary
FROM Employees
GROUP BY DepartmentID
ORDER BY AVG(Salary)


SELECT MIN(a.AverageSalary) AS MinAverageSalary
FROM (
    SELECT e.DepartmentID,
             AVG(e.Salary) AS AverageSalary
      FROM Employees AS e
      GROUP BY e.DepartmentID
     ) AS a


CREATE TABLE  #Tmp
(
    Id int PRIMARY KEY  IDENTITY ,
    Name nvarchar(100)
)

INSERT INTO #Tmp (Name) VALUES ('Aleksandra')

SELECT * FROM  #Tmp



