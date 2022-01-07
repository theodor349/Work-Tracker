CREATE PROCEDURE [dbo].[spWorkEntry_Update]
	@employerId uniqueidentifier,
	@oldStartTime DATETIME2,
	@newStartTime DATETIME2,
	@newEndTime DATETIME2
AS
BEGIN
	UPDATE [WorkEntry]
	SET [StartTime] = @newStartTime, [EndTime] = @newEndTime
	WHERE [EmployerId] = @employerId AND [StartTime] = @oldStartTime
END