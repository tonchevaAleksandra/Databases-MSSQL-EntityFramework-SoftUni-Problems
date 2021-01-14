CREATE VIEW v_HighestPeak AS
SELECT TOP (1) *
FROM Geography.dbo.Peaks
ORDER BY Elevation DESC

USE SoftUni1

INSERT INTO Towns (Name)
VALUES ('Plovdiv')

INSERT INTO Projects(name, description, startdate, enddate)
SELECT '[New] ' + Name, Description, GETDATE() AS StartDate
FROM SoftUni1.dbo.Projects
WHERE Name LIKE 'C%'

SELECT FirstName +' ' + LastName AS  FullName, Salary
INTO  Names
FROM SoftUni1.dbo.Employees;

CREATE SEQUENCE seq_MyNumbers
AS int
START WITH 0
INCREMENT BY 20

SELECT NEXT VALUE FOR seq_MyNumbers

DELETE FROM  Names
WHERE FullName LIKE 'A%'

SELECT *
FROM Names;

UPDATE Projects
SET EndDate=GETDATE()
WHERE EndDate is NULL


