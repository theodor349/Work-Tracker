CREATE PROCEDURE [dbo].[spInvoice_Update]
	@EmployerId uniqueidentifier,
	@CreationDate datetime2,
	@TotalTime BIGINT,
	@StartDate datetime2,
	@EndDate datetime2
AS 
BEGIN 
	UPDATE Invoice 
	SET [TotalTime] = @TotalTime, [StartDate] = @StartDate, [EndDate] = @EndDate
	WHERE [EmployerId] = @EmployerId AND [CreationDate] = @CreationDate
END
