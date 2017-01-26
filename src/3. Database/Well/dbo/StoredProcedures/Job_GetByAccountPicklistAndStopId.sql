CREATE PROCEDURE [dbo].[Job_GetByAccountPicklistAndStopId]
	@AccountId		VARCHAR(50),
	@PicklistId		VARCHAR(50),
	@StopId         INT

AS
BEGIN
	SELECT [Id]
      ,[Sequence]
      ,[JobTypeCode]
      ,[PHAccount]
      ,[PickListRef]
      ,[InvoiceNumber]
      ,[CustomerRef]
      ,[OrderDate]
	  ,[RoyaltyCode]
	  ,[RoyaltyCodeDesc] 
	  ,[OrdOuters] 
	  ,[InvOuters] 
	  ,[ColOuters] 
	  ,[ColBoxes] 
	  ,[ReCallPrd] 
	  ,[AllowSOCrd] 
	  ,[COD] 
	  ,[GrnProcessType]
	  ,[GrnNumber] 
	  ,[GrnRefusedReason] 
	  ,[GrnRefusedDesc] 
	  ,[AllowReOrd] 
	  ,[SandwchOrd] 
	  ,[TotalCreditValueForThreshold]
      ,[PerformanceStatusId]
      ,[Reason]
	  ,[IsDeleted]
      ,[StopId]
	  ,[ProofOfDelivery]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[Job]
  WHERE [PHAccount] = @AccountId
  AND [PickListRef] = @PicklistId
  AND [StopId] = @StopId
END