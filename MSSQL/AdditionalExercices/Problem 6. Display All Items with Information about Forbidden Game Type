SELECT I.Name AS Item,
       I.Price,
       I.MinLevel,
       GT.Name AS [Forbidden Game Type]
FROM Items AS I
left JOIN GameTypeForbiddenItems GTFI on I.Id = GTFI.ItemId
 left JOIN GameTypes GT on GT.Id = GTFI.GameTypeId
ORDER BY [Forbidden Game Type] DESC, I.Name