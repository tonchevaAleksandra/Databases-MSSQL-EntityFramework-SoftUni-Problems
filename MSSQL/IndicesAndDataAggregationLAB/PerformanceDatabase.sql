USE master
GO

CREATE DATABASE PerformanceDB
GO

USE PerformanceDB
GO

---------------------------------------------------------------------
-- Create table Authors and populate 160 000 rows in it
---------------------------------------------------------------------

CREATE TABLE Authors(
  AuthorId int NOT NULL PRIMARY KEY IDENTITY,
  AuthorName varchar(100),
)
INSERT INTO Authors(AuthorName) VALUES ('Bugs Bunny')
INSERT INTO Authors(AuthorName) VALUES ('Homer Simpson')
INSERT INTO Authors(AuthorName) VALUES ('Mickey Mouse')
INSERT INTO Authors(AuthorName) VALUES ('Bart Simpson')
INSERT INTO Authors(AuthorName) VALUES ('Garfield')
INSERT INTO Authors(AuthorName) VALUES ('Fred Flintstone')
INSERT INTO Authors(AuthorName) VALUES ('Dexter')
INSERT INTO Authors(AuthorName) VALUES ('SpongeBob SquarePants')
INSERT INTO Authors(AuthorName) VALUES ('Wile E. Coyote')
INSERT INTO Authors(AuthorName) VALUES ('Tweety Bird')
INSERT INTO Authors(AuthorName) VALUES ('Scooby-Doo')
INSERT INTO Authors(AuthorName) VALUES ('Porky Pig')
INSERT INTO Authors(AuthorName) VALUES ('Pink Panther')
INSERT INTO Authors(AuthorName) VALUES ('Winnie the Pooh')
INSERT INTO Authors(AuthorName) VALUES ('Donald Duck')
INSERT INTO Authors(AuthorName) VALUES ('Woody Woodpecker')
INSERT INTO Authors(AuthorName) VALUES ('Tom')
INSERT INTO Authors(AuthorName) VALUES ('Jerry')
INSERT INTO Authors(AuthorName) VALUES ('Daffy Duck')
INSERT INTO Authors(AuthorName) VALUES ('Stewie Griffin')

DECLARE @Counter int = 1
WHILE (SELECT COUNT(*) FROM Authors) < 200000
BEGIN
  INSERT INTO Authors(AuthorName)
  SELECT AuthorName + CONVERT(varchar, @Counter) FROM Authors
  SET @Counter = @Counter + 1
END

---------------------------------------------------------------------
-- Create table Messages and populate 40 000 000 rows in it
---------------------------------------------------------------------

CREATE TABLE Messages(
  MsgId int NOT NULL IDENTITY PRIMARY KEY,
  AuthorId int NOT NULL,
  MsgPrice int,
)

SET NOCOUNT ON
DECLARE @AuthorsCount int = (SELECT COUNT(*) FROM Authors)
DECLARE @RowCount int = 20000
WHILE @RowCount > 0
BEGIN
  INSERT INTO Messages(AuthorId, MsgPrice) VALUES(1 + (RAND() * @AuthorsCount), RAND() * 1000000)
  SET @RowCount = @RowCount - 1
END

WHILE (SELECT COUNT(*) FROM Messages) < 30000000
BEGIN
  INSERT INTO Messages(AuthorId, MsgPrice)
	SELECT AuthorId, MsgPrice FROM Messages
END
SET NOCOUNT OFF

ALTER TABLE Messages ADD CONSTRAINT FK_Messages_Authors
FOREIGN KEY (AuthorId) REFERENCES Authors(AuthorId)


---------------------------------------------------------------------
-- Check the number of rows in the tables
---------------------------------------------------------------------

SELECT COUNT(*) AS AuthorsCount FROM Authors
SELECT COUNT(*) AS MessagesCount FROM Messages

----------------------------------------------------------------------
-- Filter by indexed column (primary key has built-in clustered index)
----------------------------------------------------------------------

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT * FROM Messages
WHERE MsgId > 1000000 and MsgId < 1000100

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT * FROM Authors
WHERE AuthorId > 100000 and AuthorId < 100200

---------------------------------------------------------------------
-- Filter by non-indexed column
---------------------------------------------------------------------

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT COUNT(*) FROM Messages WHERE MsgPrice > 100000 AND MsgPrice < 200000

---------------------------------------------------------------------
-- Add indexes and filter / group by indexed column
---------------------------------------------------------------------

CREATE INDEX IDX_Messages_MsgPrice ON Messages(MsgPrice)

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT COUNT(*) FROM Messages WHERE MsgPrice > 100000 AND MsgPrice < 200000

DROP INDEX IDX_Messages_MsgPrice ON Messages