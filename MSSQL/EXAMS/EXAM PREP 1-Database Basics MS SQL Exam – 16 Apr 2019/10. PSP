SELECT PL.Name,
       PL.Seats,
       ISNULL(COUNT(P.Id),0) AS [Passengers Count]
FROM Planes AS PL
LEFT JOIN Flights F on PL.Id = F.PlaneId
LEFT JOIN Tickets T on F.Id = T.FlightId
LEFT JOIN Passengers P on P.Id = T.PassengerId
GROUP BY PL.Name, PL.Seats
ORDER BY [Passengers Count] DESC, PL.Name, PL.Seats