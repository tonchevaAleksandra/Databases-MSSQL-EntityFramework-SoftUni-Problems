THROW 500010, 'Null value for project date', 1


CREATE OR
ALTER PROC
    dbo.usp_SelectEmployeesBySeniority(@Count int OUTPUT,
                                       @Year int,
                                       @MinSalary money=10000)
AS
    IF (@Year < 0) THROW 50001, 'The year should not be a negative integer',1
    SET @Count =
            (SELECT COUNT(*)
             FROM Employees
             WHERE DATEDIFF(YEAR, HireDate, GETDATE()) > @Year
               AND @MinSalary < Employees.Salary)
SELECT COUNT(*) AS ProjectCount
FROM Projects
GO;

DECLARE @Count int;
DECLARE @CurrError int;

BEGIN TRY
    EXEC dbo.usp_SelectEmployeesBySeniority @Count OUTPUT, -2,10000
END TRY
BEGIN CATCH
    SET @CurrError = @@ERROR
END CATCH
SELECT @CurrError

SELECT *
FROM sys.messages
WHERE message_id = 8134

CREATE or alter PROCEDURE sp_InsertEmployeeForProject(@EmployeeId int,
                                             @ProjectId int)
AS
DECLARE @ProjectsCount int;
    SET @ProjectsCount = (SELECT COUNT(*)
                          FROM EmployeesProjects
                          WHERE EmployeeID = @EmployeeId);
    IF (@ProjectsCount >= 3)
        THROW 50001,'Employee already has 3 or more projects',1;
INSERT INTO EmployeesProjects(EmployeeID, ProjectID)
VALUES (@EmployeeId, @ProjectId);
GO

exec sp_InsertEmployeeForProject 1,1;