CREATE PROCEDURE [dbo].[Job_GetByAccountPicklistAndStopId]
	@AccountId		VARCHAR(50),
	@PicklistId		VARCHAR(50),
	@StopId         INT

AS
BEGIN
	SELECT TOP 1000 [Id]
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
	  ,[AllowSgCrd] 
	  ,[AllowSOCrd] 
	  ,[COD] 
	  ,[GrnNumber] 
	  ,[GrnRefusedReason] 
	  ,[GrnRefusedDesc] 
	  ,[AllowReOrd] 
	  ,[SandwchOrd] 
	  ,[ComdtyType] 
	  ,[TotalCreditValueForThreshold]
      ,[PerformanceStatusId]
      ,[ByPassReasonId]
	  ,[IsDeleted]
      ,[StopId]
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