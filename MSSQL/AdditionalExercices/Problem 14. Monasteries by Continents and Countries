--PROBLEM 1--
UPDATE Countries
SET CountryName='Burma'
WHERE CountryName = 'Myanmar'

--PROBLEM 2--
INSERT INTO Monasteries(Name, CountryCode)
VALUES ('Hanga Abbey', (SELECT Countries.CountryCode
                        FROM Countries
                        WHERE CountryName = 'Tanzania'))

--PROBLEM 3--
INSERT INTO Monasteries(Name, CountryCode)
VALUES ('Myin-Tin-Daik', (SELECT Countries.CountryCode
                          FROM Countries
                          WHERE CountryName = 'Myanmar'))

--PROBLEM 4--
SELECT C.ContinentName,
       C2.CountryName ,
      ISNULL (COUNT(M.Id),0) AS MonasteriesCount
FROM Continents AS C
  JOIN Countries C2 on C.ContinentCode = C2.ContinentCode
    AND C2.IsDeleted=0
 LEFT JOIN Monasteries M on C2.CountryCode = M.CountryCode
GROUP BY C.ContinentName, C2.CountryName
ORDER BY MonasteriesCount DESC,C2.CountryName