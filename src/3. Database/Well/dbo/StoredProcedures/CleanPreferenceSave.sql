Create PROCEDURE [dbo].[CleanPreferenceSave]
	@Days INT,
	@DateCreated DATETIME,
	@DateUpdated DATETIME,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CleanPreference]
           ([Days]
		   ,[CreatedDate]
           ,[LastUpdatedDate]
           ,[CreatedBy]
           ,[LastUpdatedBy])
     VALUES
           (@Days
		   ,@DateCreated
           ,@DateUpdated
           ,@CreatedBy
           ,@UpdatedBy);

	SELECT CAST(SCOPE_IDENTITY() as int);
END