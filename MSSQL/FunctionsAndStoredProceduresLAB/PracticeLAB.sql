DECLARE @Year smallint=2020;
DECLARE @Year2 int=2021;

SET @Year = 2022;

IF (@Year = DATEPART(YEAR, GETDATE()))
    BEGIN
        SELECT GETDATE()
        SET @Year = YEAR(GETDATE()) + 1
    END
ELSE
    IF (@Year = 2029)
        SELECT '2029!'
    ELSE
        SELECT 'No Match!'


DECLARE @Year2 smallint=2022
SELECT CASE @Year2
           WHEN 2020 THEN '2020'
           WHEN 2021 THEN '2021'
           ELSE
               'Invalid year!'
           END


USE SoftUni1

DECLARE @Year int= 1999;

WHILE @Year <= 2005
    BEGIN
        IF (@Year = 2002)
            CONTINUE
        SELECT @Year, COUNT(*)
        FROM Employees
        WHERE DATEPART(YEAR, HireDate) = @Year;
        SET @Year = @Year + 1;

        IF (@Year = 2003)
            BREAK
    END

SELECT @Year;


CREATE VIEW v_GeetSomeConstants AS
SELECT PI()         AS PI,
       GETDATE()    AS Now,
       RAND()       AS Rand,
       SQRT(10)        Sqrt10,
       POWER(2, 10) AS TwoPowerOf10;

SELECT *
FROM v_GeetSomeConstants;


/*scalar function- returns only one value*/
/*table- value functions return table - it's like view with parameters
  -inline TVF
  -multi-statement MST
 FUNCTIONS CAN NOT CHANGE THE DATABASE (INSERT, DROP, IPDATE ETC */

DECLARE @TableName nvarchar(9)='Employees';

EXEC ('select * from' +@TableName)



