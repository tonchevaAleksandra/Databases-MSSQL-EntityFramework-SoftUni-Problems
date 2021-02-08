SELECT C.CountryName,
       C2.ContinentName,
       ISNULL(COUNT(R2.Id), 0) AS RIVERSCOUNT,
       ISNULL(SUM(R2.Length), 0) AS TOTALLENGTH
FROM Countries AS C
         JOIN Continents C2 on C2.ContinentCode = C.ContinentCode
         LEFT JOIN CountriesRivers CR on C.CountryCode = CR.CountryCode
         LEFT JOIN Rivers R2 on R2.Id = CR.RiverId
GROUP BY C.CountryName, C2.ContinentName
ORDER BY RIVERSCOUNT DESC, TOTALLENGTH DESC, C.CountryName