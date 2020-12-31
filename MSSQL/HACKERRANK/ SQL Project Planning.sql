SELECT p1.Start_Date, p2.End_Date
FROM (
         SELECT Start_Date, ROW_NUMBER() OVER (ORDER BY Start_Date) RNQuery
         FROM Projects
         WHERE Start_Date NOT IN (
             SELECT END_Date
             FROM Projects)) AS p1
         JOIN (SELECT End_Date, ROW_NUMBER() OVER (ORDER BY End_Date) RNQuery
               FROM Projects
               WHERE End_Date NOT IN (
                   SELECT Start_Date
                   FROM Projects)
) AS p2 ON p1.RNQuery = p2.RNQuery
ORDER BY DATEDIFF(DAY, p1.Start_Date, p2.End_Date), p1.Start_Date
