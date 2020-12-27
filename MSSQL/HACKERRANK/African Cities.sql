SELECT c.name
FROM city AS c
         JOIN country AS co ON c.countrycode = co.code
WHERE co.continent LIKE 'Africa'