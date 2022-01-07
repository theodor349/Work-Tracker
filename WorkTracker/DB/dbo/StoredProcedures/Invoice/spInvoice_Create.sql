CREATE PROCEDURE [dbo].[spInvoice_Create]
	@EmployerId uniqueidentifier,
	@CreationDate datetime2,
	@TotalTime BIGINT,
	@StartDate datetime2,
	@EndDate datetime2
AS 
BEGIN 
	INSERT INTO 
	Invoice ([EmployerId], [CreationDate], [TotalTime], [StartDate], [EndDate])
	VALUES (@EmployerId, @CreationDate, @TotalTime, @StartDate, @EndDate)
END