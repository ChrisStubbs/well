CREATE PROCEDURE [dbo].[UserJob_Insert]
	@UserId int,
	@JobId int,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN
	  
	  MERGE INTO [dbo].[UserJob] WITH (HOLDLOCK) AS Target
	  USING
	  (
		SELECT @UserId, @JobId
	  ) AS Source
	  ON Source.JobId = Target.JobId
	  WHEN MATCHED THEN
	  UPDATE SET Target.UserId = Source.UserId, Target.UpdatedBy = @UpdatedBy, Target.DateUpdated = @DateUpdated
	  WHEN NOT MATCHED THEN
	  INSERT (UserId, JobId, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	  VALUES (@UserId, @JobId, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated);
END