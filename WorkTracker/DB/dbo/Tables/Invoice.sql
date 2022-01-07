CREATE TABLE [dbo].[Invoice]
(
	[EmployerId] UNIQUEIDENTIFIER NOT NULL, 
    [CreationDate] DATETIME2 NOT NULL, 
    [TotalTime] BIGINT NOT NULL, 
    [StartDate] DATETIME2 NOT NULL, 
    [EndDate] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Invoice_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [Employer]([Id]),
    PRIMARY KEY ([EmployerId], [CreationDate]),
    
)
