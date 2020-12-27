DECLARE @x1 decimal(18, 4)=(SELECT CAST(MIN(lat_n) AS decimal(18, 4))
                            FROM station);
DECLARE @y1 decimal(18, 4)=(SELECT CAST(MIN(long_w) AS decimal(18, 4))
                            FROM station);
DECLARE @x2 decimal(18, 4)= (SELECT CAST(MAX(lat_n) AS decimal(18, 4))
                             FROM station);
DECLARE @y2 decimal(18, 4)=(SELECT CAST(MAX(long_w) AS decimal(18, 4))
                            FROM station);
DECLARE @output decimal(18, 4) = (SELECT ABS(@x1 - @x2) + ABS(@y1 - @y2));
SELECT @output