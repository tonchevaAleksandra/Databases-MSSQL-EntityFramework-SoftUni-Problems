SELECT FirstName, Age, PhoneNumber
FROM Customers
WHERE (Age >= 21 AND FirstName LIKE '%an%')
   or (PhoneNumber like '%38' and CountryId !=
                                  (select id
                                   from Countries
                                   where Name = 'Greece'))
order by FirstName, Age desc