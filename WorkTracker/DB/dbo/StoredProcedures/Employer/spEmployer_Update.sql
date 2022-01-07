CREATE PROCEDURE [dbo].[spEmployer_Update]
	@id uniqueidentifier,
	@name NVARCHAR(64),
	@userId uniqueidentifier
AS
BEGIN
	UPDATE Employer
	SET [Name] = @name
	WHERE [Id] = @id
END
