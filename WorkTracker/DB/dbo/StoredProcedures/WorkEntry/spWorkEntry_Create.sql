CREATE PROCEDURE [dbo].[spWorkEntry_Create]
	@employerId uniqueidentifier,
	@startTime DATETIME2,
	@endTime DATETIME2
AS
BEGIN
	INSERT INTO 
	[WorkEntry] ([EmployerId], [StartTime], [EndTime])
	VALUES (@employerId, @startTime, @endTime)
END