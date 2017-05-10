CREATE PROCEDURE [dbo].[Location_Insert]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @process VARCHAR(50) = 'WellUpdate'

	DECLARE @AccountsToUpdate TABLE(
		BranchId INT,
		AccountCode VARCHAR(20),
		Name VARCHAR(50),
		AddressLine1 VARCHAR(50),
		AddressLine2 VARCHAR(50),
		Postcode VARCHAR(10)
	)
	
	-- Find all accounts where the stop has no Location_Id
	INSERT INTO @AccountsToUpdate(BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode)
	SELECT 	b.Id AS BranchId
		,Code AS AccountCode
		,a.Name 
		,a.Address1
		,a.Address2
		,a.PostCode
	FROM Account a
	JOIN [Stop] s ON s.Id = a.StopId
	JOIN [RouteHeader] rh ON rh.Id = s.RouteHeaderId
	JOIN [Branch] b ON b.Id = rh.RouteOwnerId
	WHERE s.Location_Id IS NULL
	GROUP BY b.Id, a.Code, a.Name, a.Address1, a.Address2, a.Postcode

	-- match on the branch & account code in Location
	-- insert the location if there is no match
	MERGE INTO Location AS Target
	USING (SELECT BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode FROM @AccountsToUpdate) AS Source
	(BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode)
	ON Target.BranchId = Source.BranchId AND Target.AccountCode = Source.AccountCode
	WHEN MATCHED THEN
	UPDATE SET Name = Source.Name, AddressLine1 = Source.AddressLine1, AddressLine2 = Source.AddressLine2, Postcode = Source.Postcode, LastUpdatedBy = @process, LastUpdatedDate =  GETDATE()
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate) 
	VALUES (BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode,  @process, GETDATE(), @process, GETDATE())
	;

	-- find the location for each stop with no locationid
	-- and update the stop
	DECLARE @Stops TABLE
	(Stopid INT, LocationId INT)

	INSERT INTO @Stops(Stopid, LocationId)
	SELECT 	s.Id
		,l.Id
	FROM Account a
	INNER JOIN [Stop] s ON s.Id = a.StopId
	INNER JOIN [RouteHeader] rh ON rh.Id = s.RouteHeaderId
	INNER JOIN [Branch] b ON b.Id = rh.RouteOwnerId
	INNER JOIN [Location] l ON l.BranchId = b.Id AND l.AccountCode = a.Code
	WHERE s.Location_Id IS NULL

	UPDATE S1
	SET Location_Id = S2.LocationId 
	FROM [Stop] S1
	INNER JOIN @Stops S2 ON S2.Stopid = S1.Id 

    RETURN 0;

END
