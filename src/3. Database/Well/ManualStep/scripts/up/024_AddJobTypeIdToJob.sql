IF COL_LENGTH('dbo.Job', 'JobTypeId') IS NULL
BEGIN
	-- Add new column as nullable first
	ALTER TABLE dbo.Job
	ADD JobTypeId TINYINT NULL;
	SELECT 'Added column JobTypeId' as [Add Column];
END
GO

IF (SELECT COUNT(1) JobTypeId FROM JOB WHERE JobTypeId IS NULL) > 0
BEGIN
	-- Update all JobTypeId FKs using text lookup
	update j
	set JobTypeId = jt.id
	from [Well].[dbo].[Job] j
	left outer join [well].[dbo].JobType jt
	on j.JobTypeCode = jt.Code;
	SELECT 'Updated JobTypeId values' as [Fix Data];
END
GO

IF (SELECT COLUMNPROPERTY(OBJECT_ID('dbo.Job', 'U'), 'JobTypeId', 'AllowsNull')) = 1
BEGIN
	-- Change the column to not nullable
	ALTER TABLE dbo.Job 
	ALTER COLUMN JobTypeId TINYINT NOT NULL;
	SELECT 'Make JobTypeId NOT NULL' as [Not Nullable];
END
GO

IF (SELECT count(1) FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_Job_JobType') = 0
BEGIN
	-- Add the FK constraint
	ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_JobType] FOREIGN KEY([JobTypeId])
	REFERENCES [dbo].[JobType] ([Id]);
	SELECT 'Add JobTypeId constraint' as [Add Constraints];
END
GO
