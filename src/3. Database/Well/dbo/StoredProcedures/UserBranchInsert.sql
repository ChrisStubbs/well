CREATE PROCEDURE  [dbo].[UserBranchInsert]
	@BranchId INT,
	@UserId INT,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[UserBranch](UserId, BranchId, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	VALUES (@UserId, @BranchId, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END