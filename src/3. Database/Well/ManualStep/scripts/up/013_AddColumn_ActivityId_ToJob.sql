IF COL_LENGTH('dbo.Job', 'ActivityId') IS NULL
BEGIN
	ALTER TABLE dbo.Job
	ADD ActivityId INT NULL 
	CONSTRAINT [FK_Job_Activity] 
	FOREIGN KEY ([ActivityId]) 
	REFERENCES [dbo].[Activity] ([Id]) 
END
