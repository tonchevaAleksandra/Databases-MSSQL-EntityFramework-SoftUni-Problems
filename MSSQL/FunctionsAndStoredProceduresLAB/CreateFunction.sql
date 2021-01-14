USE SoftUni1

CREATE FUNCTION udf_ProjectDurationWeeks(@StartDate DATETIME,
                                         @EndDate DATETIME)
    RETURNS int
BEGIN
    DECLARE @projectWeeks int;
    IF (@EndDate IS NULL)
        BEGIN
            SET @EndDate = GETDATE()
        END
    SET @projectWeeks = DATEDIFF(WEEK, @StartDate, @EndDate)
    RETURN @projectWeeks
END


CREATE OR
ALTER FUNCTION udf_Pow(@Base int, @Exp int)
    RETURNS decimal
AS
BEGIN
    DECLARE @result decimal(38, 0)=1;
    WHILE (@Exp > 0)
        BEGIN
            SET @result = @result * @Base;
            SET @Exp -= 1;
        END
    RETURN @result
END

SELECT dbo.udf_Pow(2, 50), POWER(2, 10)

SELECT *, dbo.udf_ProjectDurationWeeks(StartDate, EndDate) AS WeekDiff
FROM Projects

CREATE OR
ALTER FUNCTION udf_GetEmployeesCountByYear(@year int)
    RETURNS TABLE
        AS
        RETURN
            (
                SELECT *
                FROM Employees
                WHERE DATEPART(YEAR, HireDate) = @year
            )

SELECT *
FROM udf_GetEmployeesCountByYear(2001)


CREATE OR
ALTER FUNCTION udf_Squares(@count int)
    RETURNS @squares TABLE
                     (
                         Id     int PRIMARY KEY IDENTITY,
                         Square bigint
                     )
AS
BEGIN
    DECLARE @i int=1;
    WHILE (@i <= @count)
        BEGIN
            INSERT INTO @squares(Square) VALUES (@i * @i)
            SET @i+=1;
        END
    RETURN
END

SELECT *
FROM dbo.udf_Squares(10)
WHERE id > 5
ORDER BY id DESC


