CREATE PROCEDURE [dbo].[spEmployer_Delete]
	@id uniqueidentifier
AS
BEGIN
	DELETE 
	FROM Employer
	WHERE [Id] = @id
END