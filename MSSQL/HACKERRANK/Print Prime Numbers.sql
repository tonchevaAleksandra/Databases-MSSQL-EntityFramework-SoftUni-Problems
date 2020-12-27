USE master


DECLARE @prime INT=2;
DECLARE @OUTPUT nVARCHAR(2000)='';
WHILE (@prime <= 1000)
    BEGIN
        DECLARE @I INT=@prime - 1;
        DECLARE @checker INT= 1;
        WHILE (@I > 1)
            BEGIN
                IF (@prime % @I = 0)
                    begin
                    SET @checker = 0;
                    end
                SET @I-=1;
            END
        IF (@checker = 1)
            BEGIN
                SET @OUTPUT += cast( @prime as nvarchar(3)) +'&';
            END
        SET @prime+=1;
    END
set @OUTPUT= substring(@OUTPUT,1, len(@OUTPUT)-1);
SELECT @OUTPUT


