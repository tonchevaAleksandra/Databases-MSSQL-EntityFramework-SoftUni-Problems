USE SoftUni1

CREATE OR
ALTER FUNCTION ufn_GetSalaryLevel(@Salary decimal(19,4))
    RETURNS nvarchar(20)
AS
BEGIN

    IF (@Salary IS NULL) RETURN NULL
    IF (@Salary < 30000)
        RETURN 'Low';
    ELSE
        IF (@Salary <= 50000)
            RETURN 'Average';
        ELSE
            RETURN 'High';
    RETURN '';
END


SELECT dbo.ufn_GetSalaryLevel(10000)


SELECT FirstName, LastName, Salary, dbo.ufn_GetSalaryLevel(Salary) AS SalaryLevel
FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) NOT LIKE 'Average'