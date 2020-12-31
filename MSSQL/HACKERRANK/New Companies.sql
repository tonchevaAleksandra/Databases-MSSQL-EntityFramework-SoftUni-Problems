use master
SELECT c.company_code,
       c.founder,
       (SELECT COUNT(DISTINCT lm.lead_manager_code) FROM Lead_Manager AS lm WHERE c.company_code = lm.company_code),
       (SELECT COUNT(DISTINCT sm.senior_manager_code) FROM Senior_manager AS sm WHERE c.company_code = sm.company_code),
       (SELECT COUNT(DISTINCT m.manager_code) FROM manager AS m WHERE c.company_code = m.company_code),
       (SELECT COUNT(DISTINCT e.employee_code) FROM employee AS e WHERE c.company_code = e.company_code)
FROM Company AS c
GROUP BY c.company_code, c.founder
ORDER BY c.company_code