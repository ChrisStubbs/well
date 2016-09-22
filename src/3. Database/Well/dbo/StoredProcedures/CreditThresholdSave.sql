Create PROCEDURE [dbo].[CreditThresholdSave]
	@UserRoleId INT,
	@Threshold INT,
	@DateCreated DATETIME,
	@DateUpdated DATETIME,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CreditThreshold]
           ([UserRoleId]
		   ,[Threshold]
		   ,[CreatedDate]
           ,[LastUpdatedDate]
           ,[CreatedBy]
           ,[LastUpdatedBy])
     VALUES
           (@UserRoleId
		   ,@Threshold
		   ,@DateCreated
           ,@DateUpdated
           ,@CreatedBy
           ,@UpdatedBy);

	SELECT CAST(SCOPE_IDENTITY() as int);
END