CREATE PROCEDURE  [dbo].[UserByCreditThresholdGet]
	@creditThresholdId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[Id],
		[Name],
		[IdentityName],
		[JobDescription],
		[Domain],
		[ThresholdLevelId],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
	  FROM [dbo].[User]
	  WHERE ThresholdLevelId = @creditThresholdId

END