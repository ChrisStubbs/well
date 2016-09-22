Create PROCEDURE [dbo].[SeasonalDatesSave]
	@Description VARCHAR(255),
	@From DATETIME,
	@To DATETIME,
	@DateCreated DATETIME,
	@DateUpdated DATETIME,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[SeasonalDate]
           ([Description]
		   ,[From]
		   ,[To]
		   ,[CreatedDate]
           ,[LastUpdatedDate]
           ,[CreatedBy]
           ,[LastUpdatedBy])
     VALUES
           (@Description
		   ,@From
		   ,@To
		   ,@DateCreated
           ,@DateUpdated
           ,@CreatedBy
           ,@UpdatedBy);

	SELECT CAST(SCOPE_IDENTITY() as int);
END