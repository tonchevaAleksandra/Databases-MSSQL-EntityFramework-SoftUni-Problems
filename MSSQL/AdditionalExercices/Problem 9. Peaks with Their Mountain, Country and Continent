SELECT P.PeakName,M.MountainRange, C.CountryName, C2.ContinentName
FROM Peaks AS P
JOIN Mountains M on M.Id = P.MountainId
JOIN MountainsCountries MC on M.Id = MC.MountainId
JOIN Countries C on C.CountryCode = MC.CountryCode
JOIN Continents C2 on C2.ContinentCode = C.ContinentCode
ORDER BY P.PeakName, C.CountryName