USE SoftUni1

SELECT FirstName
FROM Employees
WHERE DepartmentID in (3,10)
    AND YEAR(HireDate) BETWEEN 1995 AND 2005
