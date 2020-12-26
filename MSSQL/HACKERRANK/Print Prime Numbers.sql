USE master


DECLARE @COUNT INT=1;
DECLARE @OUTPUT VARCHAR(MAX);
WHILE (@COUNT <= 1000)
    BEGIN
        IF (@COUNT = 2)
            SET @OUTPUT = CONCAT(@OUTPUT,'&', @COUNT);
        IF (@COUNT % 2 <> 0)
            BEGIN
                DECLARE @INT INT=3;
                DECLARE @END INT= SQRT(@COUNT)
                WHILE (@INT <= 1000)
                    BEGIN
                        IF (@COUNT % @END <> 0)
                            SET @OUTPUT = CONCAT(@OUTPUT,'&', @COUNT);
                        SET @INT+=2;
                    END
            END
        SET @COUNT+=1;
    END
SELECT @OUTPUT


