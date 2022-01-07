CREATE PROCEDURE [dbo].[spWorkEntry_GetLatest]
	@employerId uniqueidentifier
AS
BEGIN
	SELECT TOP 1 * 
	FROM [WorkEntry]
	WHERE [EmployerId] = @employerId
	ORDER BY [StartTime] DESC
END
