DECLARE @COUNT INT=1
WHILE(@COUNT<=20)
BEGIN
SELECT RTRIM(REPLICATE('* ', @COUNT))
SET @COUNT+=1
END