SELECT co.continent, AVG(c.population)
FROM country AS co
         JOIN city AS c ON co.code = c.countrycode
GROUP BY co.continent