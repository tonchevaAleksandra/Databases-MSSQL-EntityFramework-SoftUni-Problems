 SELECT JobDuringJourney,FULLNAME, RANKQUERY.RANK
     FROM (SELECT TC.JobDuringJourney AS JobDuringJourney,
       C.FirstName+' '+C.LastName AS FULLNAME,
      ( DENSE_RANK() over (PARTITION BY TC.JobDuringJourney ORDER BY C.BirthDate)) AS RANK
FROM Colonists AS C
JOIN TravelCards TC on C.Id = TC.ColonistId) AS RANKQUERY
WHERE RANK=2