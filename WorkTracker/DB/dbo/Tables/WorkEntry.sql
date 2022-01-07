CREATE TABLE [dbo].[WorkEntry]
(
	[EmployerId] UNIQUEIDENTIFIER NOT NULL, 
    [StartTime] DATETIME2 NOT NULL, 
    [EndTime] DATETIME2 NULL DEFAULT NULL, 
    CONSTRAINT [FK_WorkEntry_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [Employer]([Id]), 
    PRIMARY KEY ([EmployerId], [StartTime])
)
