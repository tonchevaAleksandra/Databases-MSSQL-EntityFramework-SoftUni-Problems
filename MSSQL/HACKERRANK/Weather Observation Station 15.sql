DECLARE @MAXlAT DECIMAL(18,4)=(SELECT TOP(1) CAST(MAX(LAT_N) AS DECIMAL(18,4))FROM STATION WHERE LAT_N < 137.2345)
DECLARE @OUTPUT DECIMAL(18,4)= (SELECT  CAST(LONG_W AS DECIMAL(18,4)) FROM STATION WHERE cast(LAT_N as decimal(18,4)) = @MAXlAT)
SELECT @OUTPUT