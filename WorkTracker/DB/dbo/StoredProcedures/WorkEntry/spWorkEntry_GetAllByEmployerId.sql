CREATE PROCEDURE [dbo].[spWorkEntry_GetAllByEmployerId]
	@employerId uniqueidentifier
AS
BEGIN
	SELECT * 
	FROM [WorkEntry]
	WHERE [EmployerId] = @employerId
END
