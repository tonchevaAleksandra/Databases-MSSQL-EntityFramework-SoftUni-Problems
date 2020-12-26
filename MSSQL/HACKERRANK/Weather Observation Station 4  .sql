USE master

declare @TotalRecords int=(SELECT COUNT(City) FROM STATION)
DECLARE @DistinctRecords int=(Select  Count(distinct City)from Station)
select @TotalRecords-@DistinctRecords
