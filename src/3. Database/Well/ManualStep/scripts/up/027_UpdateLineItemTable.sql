IF COL_LENGTH('dbo.LineItem', 'JobId') IS NULL
BEGIN
	ALTER TABLE [dbo].[LineItem] ALTER COLUMN [LineNumber] SMALLINT	NULL
	ALTER TABLE [dbo].[LineItem] ALTER COLUMN [CreatedDate] SMALLDATETIME NOT NULL
	ALTER TABLE [dbo].[LineItem] ALTER COLUMN [LastUpdatedDate] SMALLDATETIME NOT NULL
	ALTER TABLE [dbo].[LineItem] ALTER COLUMN [DateDeleted] SMALLDATETIME NULL
	ALTER TABLE [dbo].[LineItem] ADD [JobId] INT NULL
	
END
GO

BEGIN
	UPDATE
    LineItem
	SET
		JobId = jd.JobId 
	FROM
		LineItem li
	INNER JOIN
		JobDetail jd
	ON 
		li.Id = jd.LineItemId
END
GO