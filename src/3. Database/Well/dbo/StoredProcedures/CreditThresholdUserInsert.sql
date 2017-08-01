CREATE PROCEDURE [dbo].[CreditThresholdUserInsert]
	@UserId int,
	@CreditThresholdId int,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
	INSERT INTO CreditThresholdUser (UserId,CreditThresholdId,CreatedBy,DateCreated,UpdatedBy,DateUpdated)
	VALUES (@UserId,@CreditThresholdId,@CreatedBy,@DateCreated,@UpdatedBy,@DateUpdated)
