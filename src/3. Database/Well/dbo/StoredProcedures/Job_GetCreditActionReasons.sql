
CREATE PROCEDURE [dbo].[Job_GetCreditActionReasons]
	@PDACreditReasonId INT 
AS
BEGIN
	SELECT 
		pca.CreditActions,
		pocr.CreditReason
	FROM 
		[PODCreditActions] pca
	INNER JOIN
		PDACreditReasons pcr
	ON
		pca.PDACreditReasonId = pcr.Id
	INNER JOIN
		PODCreditReasons pocr
	ON
		pocr.Id = pca.[PODCreditReasonId]
	WHERE
		pcr.Id = @PDACreditReasonId
END
