CREATE TABLE People(
	Id Int PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture IMAGE,
	Height DECIMAL(5,2),
	[Weight] DECIMAL(5,2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People([Name],Picture, 
Height,[Weight],Gender,Birthdate, Biography)
	VALUES
	( 'Ivan', NULL, 180,75,'m', '1990/12/20', NULL),
	( 'Sanya', NULL, 165, 50,'f','1988/05/12', 'Teacher'),
	( 'Biser', NULL ,172 , 72, 'm', '1985/06/30', 'Recruiter'),
	( 'Stamen', NULL, 175, 80,'m','1980/01/25', 'Teacher'),
	( 'Sisa', NULL, 170, 62, 'f', '1992/03/01','Student')

