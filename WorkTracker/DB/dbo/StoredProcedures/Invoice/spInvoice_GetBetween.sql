CREATE PROCEDURE [dbo].[spInvoice_GetBetween]
	@EmployerId uniqueidentifier,
	@StartDate datetime2,
	@EndDate datetime2
AS
BEGIN
	SELECT * 
	FROM Invoice
	WHERE [EmployerId] = @EmployerId AND (([StartDate] <= @StartDate AND @StartDate <= [EndDate]) OR ([StartDate] <= @EndDate AND @EndDate <= [EndDate])) 
END