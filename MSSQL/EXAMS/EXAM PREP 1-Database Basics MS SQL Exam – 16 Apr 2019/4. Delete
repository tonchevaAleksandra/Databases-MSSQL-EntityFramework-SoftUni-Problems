DELETE FROM Tickets
WHERE FlightId IN (SELECT Id
    FROM Flights
    WHERE Destination LIKE 'Ayn Halagim')

DELETE FROM Flights
WHERE Destination LIKE 'Ayn Halagim'