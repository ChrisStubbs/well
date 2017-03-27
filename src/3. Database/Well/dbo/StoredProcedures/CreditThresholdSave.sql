Create PROCEDURE [dbo].[CreditThresholdSave]
	@ThresholdLevelId INT,
	@Value DECIMAL(10,2),
	@DateCreated DATETIME,
	@DateUpdated DATETIME,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CreditThreshold]
           ([ThresholdLevelId]
		   ,[Value]
		   ,[CreatedDate]
           ,[LastUpdatedDate]
           ,[CreatedBy]
           ,[LastUpdatedBy])
     VALUES
           (@ThresholdLevelId
		   ,@Value
		   ,@DateCreated
           ,@DateUpdated
           ,@CreatedBy
           ,@UpdatedBy);

	SELECT CAST(SCOPE_IDENTITY() as int);
END