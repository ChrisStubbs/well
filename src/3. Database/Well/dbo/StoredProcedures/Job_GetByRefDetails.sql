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
	  ,[PerformanceStatusId] as PerformanceStatus
	  ,[ByPassReasonId] as ByPassReason
	  ,[IsDeleted]
      ,[StopId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[Job]
  WHERE [PHAccount] = @PHAccount
  AND [PickListRef] = @PickListRef
  AND [StopId] = @StopId

END
