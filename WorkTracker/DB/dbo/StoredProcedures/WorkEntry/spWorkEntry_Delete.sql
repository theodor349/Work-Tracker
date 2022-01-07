CREATE PROCEDURE [dbo].[spWorkEntry_Delete]
	@employerId uniqueidentifier,
	@startTime DATETIME2
AS
BEGIN
	DELETE 
	FROM [WorkEntry]
	WHERE [EmployerId] = @employerId AND [StartTime] = @startTime
END