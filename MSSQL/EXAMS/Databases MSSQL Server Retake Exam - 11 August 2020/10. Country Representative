select rankQuery.Name, rankQuery.DistributorName
from (
select c.Name, d.Name as DistributorName,
       dense_rank() over (partition by c.Name order by count(i.Id) desc) as rank
from Countries as c
      join  Distributors D on c.Id = D.CountryId
     left join Ingredients I on D.Id = I.DistributorId
group by  c.Name, d.Name
) as rankQuery
where rankQuery.rank=1
 ORDER BY rankQuery.Name, rankQuery.DistributorName