CREATE PROCEDURE [dbo].[User_GetCreditThreshold]
	@Username NVARCHAR(50)
AS

SET NOCOUNT ON;

 BEGIN

	SELECT ch.Threshold
	FROM CreditThreshold ch
	INNER JOIN [User] u
	ON u.ThresholdLevelId = ch.ThresholdLevelId
	WHERE U.IdentityName = @Username
	ORDER BY ch.Threshold DESC

END
