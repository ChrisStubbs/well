IF COL_LENGTH('dbo.Job', 'WellStatusId') IS NULL
BEGIN
	ALTER TABLE dbo.Job
	ADD WellStatusId TINYINT NOT NULL DEFAULT 0;
END
GO

IF EXISTS(SELECT 1 FROM SYS.VIEWS WHERE NAME='JobStatusView' AND TYPE='V')
BEGIN
	UPDATE j
	SET WellStatusId = jsv.WellStatusId
	FROM [Well].[dbo].[Job] j
	INNER JOIN [well].[dbo].[JobStatusView] jsv
	ON j.Id = jsv.JobId;
END
GO

IF EXISTS(SELECT 1 FROM SYS.VIEWS WHERE NAME='JobStatusView' AND TYPE='V')
BEGIN
	DROP VIEW JobStatusView
END
GO
