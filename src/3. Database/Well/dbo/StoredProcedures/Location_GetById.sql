CREATE PROCEDURE [dbo].[Location_GetById]
	 @locationId INT
AS
BEGIN

	DECLARE @activities TABLE(
		DocumentNumber VARCHAR(40)
		,InitialDocument VARCHAR(40)
		,ActivityTypeId TINYINT
		,Id INT
		,LocationId INT
	)

	-- Location details
	SELECT BranchId, AccountCode, Name, AddressLine1, AddressLine2, Postcode, Id
	FROM Location 
	WHERE Id = @locationId

	-- activities for the location
	INSERT INTO @activities(DocumentNumber,InitialDocument, ActivityTypeId, Id, LocationId)
	SELECT DocumentNumber, InitialDocument, ActivityTypeId, Id, LocationId
	FROM Activity
	WHERE LocationId = @locationId 

	SELECT * FROM @activities

	-- LineItems for the Activity
	DECLARE @LineItems TABLE(
		LineNumber INT
		,  ProductCode VARCHAR(60)
		,  ProductDescription VARCHAR(100)
		,  AmendedDeliveryQuantity INT
		,  AmendedShortQuantity INT
		,  OriginalShortQuantity INT
		,  Id INT
		,  ActivityId INT
	
	)

	INSERT INTO @LineItems(LineNumber,  ProductCode,  ProductDescription,  AmendedDeliveryQuantity,  AmendedShortQuantity ,  OriginalShortQuantity , Id, ActivityId)
	SELECT   LineNumber,  ProductCode,  ProductDescription,  AmendedDeliveryQuantity,  AmendedShortQuantity,  OriginalShortQuantity, li.Id, li.ActivityId
	FROM LineItem li
	INNER JOIN @activities a on a.Id = li.ActivityId

	SELECT  LineNumber,  ProductCode,  ProductDescription,  AmendedDeliveryQuantity,  AmendedShortQuantity,  OriginalShortQuantity, Id, ActivityId
	FROM @LineItems

	-- LineItemactions for the LineItems	
	SELECT  LineItemId
        , ExceptionTypeId 
        , Quantity 
        , SourceId 
        , ReasonId 
        , ReplanDate 
        , SubmittedDate 
        , ApprovalDate
        , ApprovedBy 
		, lia.Id
	FROM LineItemAction lia
	INNER JOIN @LineItems lis ON lis.Id = lia.LineItemId
	WHERE
		lia.DateDeleted IS NULL

	RETURN 0
END





