CREATE PROCEDURE [dbo].[spGeneric_GetAll]
	@table NVARCHAR(64)
AS
BEGIN
	EXEC('SELECT * FROM [dbo].[' + @table + ']');
END