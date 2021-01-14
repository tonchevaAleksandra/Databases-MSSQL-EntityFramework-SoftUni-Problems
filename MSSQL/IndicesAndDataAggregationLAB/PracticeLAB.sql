USE tempdb

SELECT *
FROM FirstIndex;

DBCC DROPCLEANBUFFERS

USE SoftUni1

SELECT DepartmentID, MIN(Salary)
FROM Employees
GROUP BY DepartmentID

SELECT Salary, AVG(Salary)
FROM Employees
GROUP BY Salary;

SELECT DepartmentID, COUNT(*), (Salary)
FROM Employees
GROUP BY DepartmentID, Salary;

SELECT DISTINCT DepartmentID
FROM Employees

SELECT DepartmentID,
       AVG(Salary),
       COUNT(*),
       MIN(Salary),
       MAX(Salary)
FROM Employees
GROUP BY DepartmentID

SELECT DepartmentID,
       COUNT(*),
       STRING_AGG(FirstName, ' '),
       AVG(Salary)
FROM Employees
GROUP BY DepartmentID

SELECT Name,
       SUM(Salary) AS TotalSalary,
       COUNT(*)    AS CountEmployees
FROM Employees
         JOIN Departments d ON Employees.DepartmentID = d.DepartmentID
GROUP BY Name
ORDER BY Name

SELECT DepartmentID, SUM(Salary)
FROM Employees
GROUP BY DepartmentID

SELECT DepartmentID, count(*)
FROM Employees
where Salary>10000
GROUP BY DepartmentID
HAVING count(*)>=10/*like WHERE after grouping*/







