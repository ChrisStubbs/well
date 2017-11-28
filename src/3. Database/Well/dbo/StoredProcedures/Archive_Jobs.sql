CREATE PROCEDURE [dbo].[Archive_Jobs]
	@JobIds IntTableType READONLY,
    @ArchiveDate DateTime
AS
	--LineItemActionComment
	DELETE c
	OUTPUT	Deleted.[Id]
			,@@SERVERNAME + '.' + DB_NAME()
			,Deleted.[LineItemActionId]
			,Deleted.[CommentReasonId]
			,Deleted.[FromQty]
			,Deleted.[ToQty]
			,Deleted.[CreatedBy]
			,Deleted.[DateCreated]
			,Deleted.[UpdatedBy]
			,Deleted.[DateUpdated]
			,Deleted.[DateDeleted]
			,Deleted.[DeletedByImport]
			,@ArchiveDate
	INTO [$(WellArchive)].[dbo].LineItemActionComment
			([Id]
			,DataSource
			,[LineItemActionId]
			,[CommentReasonId]
			,[FromQty]
			,[ToQty]
			,[CreatedBy]
			,[DateCreated]
			,[UpdatedBy]
			,[DateUpdated]
			,[DateDeleted]
			,[DeletedByImport]
			,[ArchiveDate])
	FROM dbo.LineItemActionComment c
	INNER JOIN LineItemAction lia ON lia.Id = c.LineItemActionId
	INNER JOIN LineItem li ON li.Id = lia.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItemActionComment')
	PRINT ('----------------')

	--LineItemAction
	DELETE lia
	OUTPUT Deleted.[Id]
		  ,@@SERVERNAME + '.' + DB_NAME()
		  ,Deleted.[ExceptionTypeId]
		  ,Deleted.[Quantity]
		  ,Deleted.[SourceId]
		  ,Deleted.[ReasonId]
		  ,Deleted.[ReplanDate]
		  ,Deleted.[SubmittedDate]
		  ,Deleted.[ApprovalDate]
		  ,Deleted.[ApprovedBy]
		  ,Deleted.[LineItemId]
		  ,Deleted.[Originator]
		  ,Deleted.[ActionedBy]
		  ,Deleted.[DeliveryActionId]
		  ,Deleted.[PDAReasonDescription]
		  ,Deleted.[CreatedBy]
		  ,Deleted.[CreatedDate]
		  ,Deleted.[LastUpdatedBy]
		  ,Deleted.[LastUpdatedDate]
		  ,Deleted.[DateDeleted]
		  ,Deleted.[DeletedByImport]
		  ,@ArchiveDate
	INTO [$(WellArchive)].[dbo].LineItemAction
		  ([Id]
		  ,DataSource
		  ,[ExceptionTypeId]
		  ,[Quantity]
		  ,[SourceId]
		  ,[ReasonId]
		  ,[ReplanDate]
		  ,[SubmittedDate]
		  ,[ApprovalDate]
		  ,[ApprovedBy]
		  ,[LineItemId]
		  ,[Originator]
		  ,[ActionedBy]
		  ,[DeliveryActionId]
		  ,[PDAReasonDescription]
		  ,[CreatedBy]
		  ,[CreatedDate]
		  ,[LastUpdatedBy]
		  ,[LastUpdatedDate]
		  ,[DateDeleted]
		  ,[DeletedByImport]
		  ,[ArchiveDate])
	FROM dbo.LineItemAction lia
	INNER JOIN LineItem li ON li.Id = lia.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItemAction')
	PRINT ('----------------')

	--JobDetailDamage
	DELETE jdd
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[JobDetailId]
		,Deleted.[Qty]
		,Deleted.[DateDeleted]
		,Deleted.[DeletedByImport]
		,Deleted.[JobDetailSourceId]
		,Deleted.[JobDetailReasonId]
		,Deleted.[DamageActionId]
		,Deleted.[DamageStatus]
		,Deleted.[PdaReasonDescription]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].JobDetailDamage	
		([Id]
		,DataSource
		,[JobDetailId]
		,[Qty]
		,[DateDeleted]
		,[DeletedByImport]
		,[JobDetailSourceId]
		,[JobDetailReasonId]
		,[DamageActionId]
		,[DamageStatus]
		,[PdaReasonDescription]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[ArchiveDate])
	FROM dbo.JobDetailDamage jdd
	INNER JOIN JobDetail Jd ON Jd.Id = jdd.JobDetailId 
	INNER JOIN @JobIds jobIds ON jd.JobId = jobIds.Value
	PRINT ('Deleted JobDetailDamage')
	PRINT ('----------------')

	SELECT jd.BagId 
	INTO #Bag 
	FROM dbo.JobDetail jd
	INNER JOIN LineItem li ON li.id = jd.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value

	--JobDetail
	DELETE jd
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[LineNumber]
		,Deleted.[PHProductCode]
		,Deleted.[OriginalDespatchQty]
		,Deleted.[DeliveredQty]
		,Deleted.[ProdDesc]
		,Deleted.[OrderedQty]
		,Deleted.[ShortQty]
		,Deleted.[ShortsActionId]
		,Deleted.[JobDetailReasonId]
		,Deleted.[JobDetailSourceId]
		,Deleted.[UnitMeasure]
		,Deleted.[PHProductType]
		,Deleted.[PackSize]
		,Deleted.[SingleOrOuter]
		,Deleted.[SSCCBarcode]
		,Deleted.[SubOuterDamageTotal]
		,Deleted.[SkuGoodsValue]
		,Deleted.[NetPrice]
		,Deleted.[JobId]
		,Deleted.[ShortsStatus]
		,Deleted.[LineDeliveryStatus]
		,Deleted.[IsHighValue]
		,Deleted.[DateLife]
		,Deleted.[DateDeleted]
		,Deleted.[DeletedByImport]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,Deleted.[LineItemId]
		,Deleted.[BagId]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].JobDetail 
		([Id]
		,DataSource
		,[LineNumber]
		,[PHProductCode]
		,[OriginalDespatchQty]
		,[DeliveredQty]
		,[ProdDesc]
		,[OrderedQty]
		,[ShortQty]
		,[ShortsActionId]
		,[JobDetailReasonId]
		,[JobDetailSourceId]
		,[UnitMeasure]
		,[PHProductType]
		,[PackSize]
		,[SingleOrOuter]
		,[SSCCBarcode]
		,[SubOuterDamageTotal]
		,[SkuGoodsValue]
		,[NetPrice]
		,[JobId]
		,[ShortsStatus]
		,[LineDeliveryStatus]
		,[IsHighValue]
		,[DateLife]
		,[DateDeleted]
		,[DeletedByImport]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[LineItemId]
		,[BagId]
		,[ArchiveDate])
	FROM dbo.JobDetail jd
	INNER JOIN @JobIds jobIds ON jd.JobId = jobIds.Value
	PRINT ('Deleted JobDetail')
	PRINT ('----------------')


	--JobDetail
	DELETE jd
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[LineNumber]
		,Deleted.[PHProductCode]
		,Deleted.[OriginalDespatchQty]
		,Deleted.[DeliveredQty]
		,Deleted.[ProdDesc]
		,Deleted.[OrderedQty]
		,Deleted.[ShortQty]
		,Deleted.[ShortsActionId]
		,Deleted.[JobDetailReasonId]
		,Deleted.[JobDetailSourceId]
		,Deleted.[UnitMeasure]
		,Deleted.[PHProductType]
		,Deleted.[PackSize]
		,Deleted.[SingleOrOuter]
		,Deleted.[SSCCBarcode]
		,Deleted.[SubOuterDamageTotal]
		,Deleted.[SkuGoodsValue]
		,Deleted.[NetPrice]
		,Deleted.[JobId]
		,Deleted.[ShortsStatus]
		,Deleted.[LineDeliveryStatus]
		,Deleted.[IsHighValue]
		,Deleted.[DateLife]
		,Deleted.[DateDeleted]
		,Deleted.[DeletedByImport]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,Deleted.[LineItemId]
		,Deleted.[BagId]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].JobDetail 
		([Id]
		,DataSource
		,[LineNumber]
		,[PHProductCode]
		,[OriginalDespatchQty]
		,[DeliveredQty]
		,[ProdDesc]
		,[OrderedQty]
		,[ShortQty]
		,[ShortsActionId]
		,[JobDetailReasonId]
		,[JobDetailSourceId]
		,[UnitMeasure]
		,[PHProductType]
		,[PackSize]
		,[SingleOrOuter]
		,[SSCCBarcode]
		,[SubOuterDamageTotal]
		,[SkuGoodsValue]
		,[NetPrice]
		,[JobId]
		,[ShortsStatus]
		,[LineDeliveryStatus]
		,[IsHighValue]
		,[DateLife]
		,[DateDeleted]
		,[DeletedByImport]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[LineItemId]
		,[BagId]
		,[ArchiveDate])
	FROM dbo.JobDetail jd
	INNER JOIN LineItem li ON li.id = jd.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted JobDetail')
	PRINT ('----------------')
	
	--LineItem
	DELETE	li
	OUTPUT	Deleted.[Id]
	        ,@@SERVERNAME + '.' + DB_NAME()
			,Deleted.[LineNumber]
			,Deleted.[ProductCode]
			,Deleted.[ProductDescription]
			,Deleted.[AmendedDeliveryQuantity]
			,Deleted.[AmendedShortQuantity]
			,Deleted.[OriginalShortQuantity]
			,Deleted.[BagId]
			,Deleted.[ActivityId]
			,Deleted.[CreatedBy]
			,Deleted.[CreatedDate]
			,Deleted.[LastUpdatedBy]
			,Deleted.[LastUpdatedDate]
			,Deleted.[DateDeleted]
			,Deleted.[DeletedByImport]
			,Deleted.[JobId]
			,@ArchiveDate
	INTO  [$(WellArchive)].[dbo].LineItem
		([Id]
		,DataSource
		,[LineNumber]
		,[ProductCode]
		,[ProductDescription]
		,[AmendedDeliveryQuantity]
		,[AmendedShortQuantity]
		,[OriginalShortQuantity]
		,[BagId]
		,[ActivityId]
		,[CreatedBy]
		,[CreatedDate]
		,[LastUpdatedBy]
		,[LastUpdatedDate]
		,[DateDeleted]
		,[DeletedByImport]
		,[JobId]
		,[ArchiveDate])
	FROM dbo.LineItem li
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItem')
	PRINT ('----------------')

	--Bag
	DELETE b
	OUTPUT	Deleted.[Id]
			,@@SERVERNAME + '.' + DB_NAME()
			,Deleted.[Barcode]
			,Deleted.[Description]
			,Deleted.[CreatedBy]
			,Deleted.[CreatedDate]
			,Deleted.[LastUpdatedBy]
			,Deleted.[LastUpdatedDate]
			,Deleted.[DateDeleted]
			,@ArchiveDate
	INTO [$(WellArchive)].[dbo].Bag	
			([Id]
			,DataSource
			,[Barcode]
			,[Description]
			,[CreatedBy]
			,[CreatedDate]
			,[LastUpdatedBy]
			,[LastUpdatedDate]
			,[DateDeleted]
			,[ArchiveDate])
	FROM dbo.Bag b
	INNER JOIN #Bag bid on bid.BagId = b.Id
	PRINT ('Deleted Bag')
	PRINT ('----------------')

	--JobResolutionStatus
	DELETE jrs
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[Status]
		,Deleted.[Job]
		,Deleted.[By]
		,Deleted.[On]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].JobResolutionStatus 
		([Id]
		,DataSource
		,[Status]
		,[Job]
		,[By]
		,[On]	  
		,[ArchiveDate])
	FROM dbo.JobResolutionStatus jrs
	INNER JOIN @JobIds jobIds ON jrs.Job = jobIds.Value
	PRINT ('Deleted JobResolutionStatus')
	PRINT ('----------------')

	--UserJob
	DELETE uj
	OUTPUT Deleted. [Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[UserId]
		,Deleted.[JobId]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].UserJob	
		([Id]
		,DataSource
		,[UserId]
		,[JobId]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[ArchiveDate])
	FROM dbo.UserJob uj
	INNER JOIN @JobIds jobIds ON uj.JobId = jobIds.Value
	PRINT ('Deleted UserJob')
	PRINT ('----------------')

	--Job
	DELETE j
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[Sequence]
		,Deleted.[JobTypeCode]
		,Deleted.[PHAccount]
		,Deleted.[PickListRef]
		,Deleted.[InvoiceNumber]
		,Deleted.[CustomerRef]
		,Deleted.[OrderDate]
		,Deleted.[RoyaltyCode]
		,Deleted.[RoyaltyCodeDesc]
		,Deleted.[OrdOuters]
		,Deleted.[InvOuters]
		,Deleted.[ColOuters]
		,Deleted.[ColBoxes]
		,Deleted.[ReCallPrd]
		,Deleted.[AllowSOCrd]
		,Deleted.[COD]
		,Deleted.[GrnProcessType]
		,Deleted.[GrnNumber]
		,Deleted.[GrnRefusedReason]
		,Deleted.[GrnRefusedDesc]
		,Deleted.[AllowReOrd]
		,Deleted.[SandwchOrd]
		,Deleted.[PerformanceStatusId]
		,Deleted.[Reason]
		,Deleted.[DateDeleted]
		,Deleted.[DeletedByImport]
		,Deleted.[StopId]
		,Deleted.[ActionLogNumber]
		,Deleted.[OuterCount]
		,Deleted.[OuterDiscrepancyFound]
		,Deleted.[TotalOutersOver]
		,Deleted.[TotalOutersShort]
		,Deleted.[Picked]
		,Deleted.[InvoiceValue]
		,Deleted.[ProofOfDelivery]
		,Deleted.[DetailOutersOver]
		,Deleted.[DetailOutersShort]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,Deleted.[JobStatusId]
		,Deleted.[ActivityId]
		,Deleted.[ResolutionStatusId]
		,Deleted.[JobTypeId]
		,Deleted.[WellStatusId]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].Job
	  ([Id]
	  ,DataSource
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
      ,[PerformanceStatusId]
      ,[Reason]
      ,[DateDeleted]
      ,[DeletedByImport]
      ,[StopId]
      ,[ActionLogNumber]
      ,[OuterCount]
      ,[OuterDiscrepancyFound]
      ,[TotalOutersOver]
      ,[TotalOutersShort]
      ,[Picked]
      ,[InvoiceValue]
      ,[ProofOfDelivery]
      ,[DetailOutersOver]
      ,[DetailOutersShort]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[JobStatusId]
      ,[ActivityId]
      ,[ResolutionStatusId]
      ,[JobTypeId]
      ,[WellStatusId]
      ,[ArchiveDate])
	FROM dbo.Job j
	INNER JOIN @JobIds jobIds ON j.Id = jobIds.Value
	PRINT ('Job')
	PRINT ('----------------')