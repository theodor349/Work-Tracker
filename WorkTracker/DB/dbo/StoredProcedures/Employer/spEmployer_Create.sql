CREATE PROCEDURE [dbo].[spEmployer_Create]
	@id uniqueidentifier,
	@name NVARCHAR(64),
	@userId uniqueidentifier
AS 
BEGIN 
	INSERT INTO 
	Employer ([Id], [Name], [UserId])
	VALUES (@id, @name, @userId)
END
