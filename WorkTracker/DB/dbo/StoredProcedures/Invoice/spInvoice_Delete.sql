CREATE PROCEDURE [dbo].[spInvoice_Delete]
	@EmployerId uniqueidentifier,
	@CreationDate datetime2
AS 
BEGIN 
	DELETE
	FROM Invoice 
	WHERE [EmployerId] = @EmployerId AND [CreationDate] = @CreationDate
END
