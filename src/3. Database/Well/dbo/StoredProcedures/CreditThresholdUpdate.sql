CREATE PROCEDURE [dbo].[CreditThresholdUpdate]
	@Id int,
	@Threshold DECIMAL(10,2),
	@DateUpdated DATETIME,
	@UpdatedBy VARCHAR(50)
AS
	UPDATE CreditThreshold
	SET 
	Threshold = @Threshold, 
	LastUpdatedDate = @DateUpdated,
	LastUpdatedBy = @UpdatedBy
	WHERE Id = @Id

