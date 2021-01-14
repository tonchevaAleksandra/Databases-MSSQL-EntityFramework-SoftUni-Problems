USE Bank

CREATE PROC usp_TransferFunds(@FromAccountID int, @ToAccountID int, @Amount money)
AS
    BEGIN TRANSACTION
    IF (@Amount <= 0)
        BEGIN
            --  ROLLBACK -revert the transaction that has began
            THROW 50004,'Invalid amount value.',1;
            -- return
        END
    IF ((SELECT COUNT(*)
         FROM Accounts
         WHERE Id = @FromAccountID) != 1)
        BEGIN
            THROW 50001, 'Invalid account sender.',1;
        END
    IF ((SELECT COUNT(*)
         FROM Accounts
         WHERE Id = @ToAccountID) != 1)
        BEGIN
            THROW 50002, 'Invalid account receiver.',1;
        END
    IF (SELECT Balance
        FROM Accounts
        WHERE id = @FromAccountID )< @Amount
        BEGIN
            THROW 50003,'Insufficient funds to execute this transaction.',1;
        END

UPDATE Accounts
SET Balance=Balance - @Amount
WHERE id = @FromAccountID
UPDATE Accounts
SET Balance= Balance + @Amount
WHERE id = @ToAccountID
    COMMIT
GO

USE SoftUni1

CREATE OR ALTER TRIGGER tr_TownsUpdate
    ON towns
    FOR UPDATE
    AS
    IF (EXISTS(SELECT *
               FROM inserted
               WHERE Name IS NULL
                  OR LEN(Name) = 0))
        BEGIN
            RAISERROR ('Town name cannot be empty.',16,1)

            RETURN
        END

UPDATE Towns
SET Name=''
WHERE TownID = 1

USE Bank

CREATE OR ALTER TRIGGER tr_SetIsDeletedOnDelete
    ON AccountHolders
    INSTEAD OF DELETE
    AS
    UPDATE ah
    SET ah.IsDeleted=1
    FROM AccountHolders AS ah
             JOIN deleted AS d ON ah.Id = d.Id

GO

SELECT *
FROM AccountHolders

CREATE OR ALTER TRIGGER tr_SetIsDeletedOnDelete
    ON AccountHolders
    INSTEAD OF DELETE
    AS
    UPDATE AccountHolders
    SET IsDeleted=1
    WHERE id IN (SELECT id FROM deleted)
GO

CREATE TABLE Logs
(
    Id        int PRIMARY KEY IDENTITY,
    AccountId int REFERENCES Accounts (Id),
    OldAmount money NOT NULL,
    NewAmount money NOT NULL,
    UpdatedOn datetime,
    UpdatedBy nvarchar(max)
)

CREATE TRIGGER tr_AddToLogsOnAccountUpdate
    ON Accounts
    FOR UPDATE
    AS
    INSERT INTO Logs(AccountId, OldAmount, NewAmount, UpdatedOn, UpdatedBy)
    SELECT i.Id, d.Balance, i.Balance, GETDATE(), CURRENT_USER
    FROM inserted AS i
             JOIN deleted AS d ON i.Id = d.Id
    WHERE i.Balance != d.Balance
GO


exec usp_TransferFunds 12, 15,10000


SELECT * FROM Accounts
SELECT * FROM Logs

drop TRIGGER tr_SetIsDeletedOnDelete
drop TRIGGER tr_AddToLogsOnAccountUpdate
drop table Logs