CREATE PROCEDURE [dbo].[SeasonalDatesGetAll]
AS
BEGIN
SELECT
	s.[Id], s.[Description], s.[From], s.[To], s.[CreatedBy], s.[CreatedDate], s.[LastUpdatedBy], s.[LastUpdatedDate]
FROM
	 [dbo].[SeasonalDate] s 
		   
END