USE SoftUni1
GO

CREATE PROC dbo.usp_SelectEmployeesBySeniority
AS
SELECT *
FROM Employees
WHERE DATEDIFF(YEAR, HireDate, GETDATE()) > 19
GO;

exec dbo.usp_SelectEmployeesBySeniority

exec sp_monitor

exec sp_depends 'dbo.udf_GetEmployeesCountByYear'

exec sp_columns 'Employees'

/*Stored Procedure with parameters*/

CREATE or alter PROC
    dbo.usp_SelectEmployeesBySeniority(@Count int OUTPUT ,@Year int=15,@MinSalary money=10000)
AS
    set @Count=
(SELECT count(*)
FROM Employees as e
WHERE DATEDIFF(YEAR, HireDate, GETDATE()) > @Year
    and @MinSalary<Employees.Salary)
SELECT count(*) as ProjectCount
FROM Projects
GO;

DECLARE @Count int;
exec usp_SelectEmployeesBySeniority @Count OUTPUT ;
SELECT @Count



