ASP.NET Core MVC w/o EF

- SQL server connection without Entity Framework
- CRUD operations using SQL stored Procedures 

----------------

Database MoviesDB:
[MovieID] [int] IDENTITY(1,1) NOT NULL,
[Title] [varchar](100) NULL,
[Director] [varchar](100) NULL,
[Rating] [int] NULL,

--------------------

Stored procedures:

1) 
CREATE PROCEDURE [dbo].[MovieAddOrEdit]
	@MovieID INT,
	@Title VARCHAR(50),
	@Director VARCHAR(50),
	@Rating INT
AS
BEGIN
	SET NOCOUNT ON;

	IF @MovieID = 0
	BEGIN 
		INSERT INTO Movies(Title,Director,Rating)
		VALUES(@Title,@Director,@Rating)
	END
	ELSE
	BEGIN
		UPDATE Movies 
		SET
			Title=@Title,
			Director=@Director,
			Rating=@Rating
		WHERE MovieID = @MovieID
	END
END
GO

----------------------------------
2)
 
CREATE PROCEDURE [dbo].[MovieViewAll] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * FROM Movies 
END
GO

----------------------------------
3)

CREATE PROCEDURE [dbo].[MovieViewByID] 
	@MovieID INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM Movies WHERE MovieID = @MovieID
END
GO

----------------------------------
4)

CREATE PROCEDURE [dbo].[MovieDeleteByID] 
	@MovieID INT
AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE Movies WHERE MovieID = @MovieID
END
GO




