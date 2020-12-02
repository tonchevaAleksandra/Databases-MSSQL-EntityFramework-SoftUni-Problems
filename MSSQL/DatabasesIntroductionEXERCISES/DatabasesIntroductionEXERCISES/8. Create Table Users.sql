CREATE TABLE Users(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX) 
	CHECK(DATALENGTH(ProfilePicture)<=900*1024),
	LastLoginTime DATETIME2 NOT NULL,
	IsDeleted BIT NOT NULL
)

INSERT INTO Users(Username,[Password],ProfilePicture,
LastLoginTime,IsDeleted)
		VALUES
			('stiv.31','somepass',NULL, '05.19.2020',0),
			('stiv.32','somepass',NULL, '05.19.2020',1),
			('stiv.33','somepass',NULL, '05.19.2020',0),
			('stiv.34','somepass',NULL, '05.19.2020',1),
			('stiv.35','somepass',NULL, '05.19.2020',0)

