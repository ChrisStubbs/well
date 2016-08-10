CREATE PROCEDURE [dbo].[UserJob_Insert]
	@UserId int,
	@JobId int,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN
	  INSERT INTO [dbo].[UserJob] (UserId, JobId, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	  VALUES (@UserId, @JobId, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)
END