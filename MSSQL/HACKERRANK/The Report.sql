SELECT (CASE
            WHEN g.grade < 8 THEN NULL
            ELSE s.name
    END) AS Sname,
       g.grade,
       s.marks
FROM students AS s
         JOIN grades AS g ON s.marks BETWEEN g.min_mark AND g.max_mark
ORDER BY g.grade DESC, Sname, s.marks