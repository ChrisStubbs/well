CREATE PROCEDURE JobResolutionStatus_Insert
	@Status	VARCHAR(30),
	@JobId INT,
	@CreatedBy VARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON;

	 INSERT 
		[dbo].[JobResolutionStatus]
	 ([Status], [Job], [By], [On])
	 VALUES(@Status, @JobId, @CreatedBy, GETDATE())

	 SELECT CAST(SCOPE_IDENTITY() as int);
END
