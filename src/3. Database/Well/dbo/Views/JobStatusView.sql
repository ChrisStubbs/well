GO  
--Set the options to support indexed views.  
--SET NUMERIC_ROUNDABORT OFF;  
--GO
--SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT,  
--    QUOTED_IDENTIFIER, ANSI_NULLS ON; 
--GO

CREATE VIEW [dbo].[JobStatusView]
WITH SCHEMABINDING
AS
SELECT j.Id AS JobId
, js.Id AS JobStatusId
 ,CASE js.Id
	WHEN 1 THEN 1  -- Awaiting Invoice   = Planned
	WHEN 2 THEN 2  -- InComplete         = Invoiced
	WHEN 8 THEN 4  -- Bypassed           = Bypassed
	ELSE 3         -- all other statuses = Complete
  END	AS WellStatusId
FROM dbo.Job j 
INNER JOIN dbo.JobStatus js on j.JobStatusId = js.Id 


GO
--Create index on the view

CREATE UNIQUE CLUSTERED INDEX JobStatusView_Ix   
    ON [dbo].[JobStatusView] (JobId);  


