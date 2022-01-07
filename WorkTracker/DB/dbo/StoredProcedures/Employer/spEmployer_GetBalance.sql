CREATE PROCEDURE [dbo].[spEmployer_GetBalance]
	@EmployerId uniqueidentifier,
	@BeforeDate datetime2
AS 
BEGIN 
	SELECT Entries.EmployerId, Entries.TotalTime, ISNULL(Invoiced.TotalInvoiced, 0) AS TotalInvoiced
	FROM  (
			SELECT [EmployerId], SUM([TimeSpan]) AS TotalTime
			FROM (
				SELECT [EmployerId], DATEDIFF_BIG(nanosecond, [StartTime], [EndTime]) / 100 AS TimeSpan 
				FROM WorkEntry
				WHERE [StartTime] < @BeforeDate
				) AS WorkEntries
			WHERE [EmployerId] = @EmployerId
			GROUP BY [EmployerId]
		) AS Entries
		LEFT JOIN
		(
			SELECT [EmployerId], SUM([TotalTime]) AS TotalInvoiced
			FROM Invoice 
			WHERE [EmployerId] = @EmployerId
			GROUP BY [EmployerId]
		) AS Invoiced
		ON Entries.EmployerId = Invoiced.EmployerId
END
