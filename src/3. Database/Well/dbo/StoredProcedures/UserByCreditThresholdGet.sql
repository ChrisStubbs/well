CREATE PROCEDURE  [dbo].[UserByCreditThresholdGet]
	@creditThresholdId INT
AS
BEGIN

	SET NOCOUNT ON;


	SELECT 
		u.[Id],
		u.[Name],
		u.[IdentityName],
		u.[JobDescription],
		u.[Domain],
		u.[ThresholdLevelId],
		u.[CreatedBy],
		u.[DateCreated],
		u.[UpdatedBy],
		u.[DateUpdated],
		u.[Version]
	  FROM [dbo].[User] u
	  JOIN dbo.CreditThreshold ct on ct.ThresholdLevelId = u.ThresholdLevelId
	  WHERE ct.Id = @creditThresholdId

END