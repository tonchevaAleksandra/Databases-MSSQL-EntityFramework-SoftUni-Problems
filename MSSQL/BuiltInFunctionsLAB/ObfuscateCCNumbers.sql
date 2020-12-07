USE Demo

CREATE VIEW v_PublicPaymentInfo
AS
SELECT CustomerID,
       FirstName,
       LastName,
       LEFT(PaymentNumber, 6) +
       REPLICATE('*',
                 LEN(PaymentNumber) - 6) AS PaymentNumber
FROM Customers


