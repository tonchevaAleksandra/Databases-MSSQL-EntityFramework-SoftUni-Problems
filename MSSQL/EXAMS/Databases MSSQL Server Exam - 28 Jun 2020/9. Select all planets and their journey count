SELECT P.Name, COUNT(J.Id) AS C2
FROM Planets AS P
JOIN Spaceports S on P.Id = S.PlanetId
JOIN Journeys J on S.Id = J.DestinationSpaceportId
GROUP BY P.Name
ORDER BY C2 DESC , P.Name