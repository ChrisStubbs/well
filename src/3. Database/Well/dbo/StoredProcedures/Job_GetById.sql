﻿CREATE PROCEDURE [dbo].[Job_GetById]
	@Id INT

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
	  ,[GrnProcessType]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
	  ,[JobStatusId] as JobStatus
  FROM [dbo].[Job]
  WHERE [Id] = @Id
END
