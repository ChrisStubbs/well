CREATE PROCEDURE  [dbo].[PendingCreditCountByUserGet]
	@identityName varchar(255)
AS
BEGIN

	SET NOCOUNT ON;

	select
		count(1)
	from
		PendingCreditToUser p
	join
		[User] u on u.Id = p.userId
	where
		u.IdentityName = @identityName

END