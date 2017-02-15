CREATE PROCEDURE  [dbo].[PendingCreditCountByUserGet]
	@identityName varchar(255)
AS
BEGIN

	SET NOCOUNT ON;

	select
		count(1)
	from
		[PendingCredit] p
	inner join [UserJob] uj on uj.JobId = p.JobId
	inner join [User] u on u.Id = uj.UserId
	where
		u.IdentityName = @identityName AND p.IsDeleted = 0

END