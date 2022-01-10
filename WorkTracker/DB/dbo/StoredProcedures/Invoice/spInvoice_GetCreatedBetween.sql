CREATE PROCEDURE [dbo].[spInvoice_GetCreatedBetween]
	@EmployerId uniqueidentifier,
	@StartDate datetime2,
	@EndDate datetime2
AS
BEGIN
	SELECT * 
	FROM Invoice
	WHERE 
		[EmployerId] = @EmployerId 
		AND 
			(@StartDate <= [CreationDate] AND [CreationDate] <= @EndDate) 
END
