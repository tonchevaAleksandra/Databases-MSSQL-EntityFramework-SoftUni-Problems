SELECT S.Name
FROM (Students S
         JOIN Friends F ON s.Id = f.Id
         JOIN Packages P1 ON S.ID = P1.ID
         JOIN Packages P2 ON F.Friend_ID = P2.ID)
WHERE p1.salary < p2.salary
ORDER BY P2.Salary;