CREATE PROCEDURE [dbo].[Job_GetByRefDetails]
	@PHAccount NVARCHAR(40),
	@PickListRef NVARCHAR(40),
	@StopId INT
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
	  ,[GrnNumber] 
	  ,[GrnRefusedReason] 
	  ,[GrnRefusedDesc] 
	  ,[AllowReOrd] 
	  ,[SandwchOrd] 
	  ,[TotalCreditValueForThreshold]
	  ,[PerformanceStatusId] as PerformanceStatus
	  ,[Reason]
	  ,[IsDeleted]
      ,[StopId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
	  ,[JobStatusId] as JobStatus
  FROM [dbo].[Job]
  WHERE [PHAccount] = @PHAccount
  AND [PickListRef] = @PickListRef
  AND [StopId] = @StopId

END
