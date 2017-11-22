IF COL_LENGTH('dbo.JobDetail', 'UpliftAction_Id') IS NULL
BEGIN
	ALTER TABLE dbo.JobDetail
	ADD UpliftAction_Id TINYINT NULL 
	CONSTRAINT [FK_JobDetail_UpliftAction] 
	FOREIGN KEY ([UpliftAction_Id]) 
	REFERENCES [dbo].[UpliftAction] ([Id]) 
END
