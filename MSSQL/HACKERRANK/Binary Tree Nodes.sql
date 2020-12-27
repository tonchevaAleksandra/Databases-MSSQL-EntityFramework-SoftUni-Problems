SELECT CAST(N AS NVARCHAR(10)) + ' ' +
       CASE
           WHEN P IS NULL THEN 'Root'
           WHEN N IN (SELECT DISTINCT P
                        FROM BST) THEN 'Inner'
           ELSE 'Leaf'
           END
    FROM BST AS b
    ORDER BY N