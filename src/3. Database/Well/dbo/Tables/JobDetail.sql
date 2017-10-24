
CREATE TABLE [dbo].[JobDetail]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineNumber] INT NOT NULL,
	[PHProductCode] VARCHAR(60) NULL,
	[OriginalDespatchQty] INT NULL,
	[DeliveredQty] INT  NULL,
	[ProdDesc] VARCHAR(100) NULL,
	[OrderedQty] INT NULL,
	[ShortQty] INT NULL,
	[ShortsActionId] INT NULL,
	[JobDetailReasonId] TINYINT NOT NULL,
	[JobDetailSourceId] TINYINT NOT NULL,
	[UnitMeasure] VARCHAR(50) NULL,
	[PHProductType] VARCHAR(50) NULL,
	[PackSize] VARCHAR(50) NULL,
	[SingleOrOuter] VARCHAR(10) NULL,
	[SSCCBarcode] VARCHAR(50) NULL,
	[SubOuterDamageTotal] INT NULL,
	[SkuGoodsValue] FLOAT NOT NULL,
	[NetPrice] FLOAT NULL,
	[JobId] INT NOT NULL,
	[ShortsStatus] INT NOT NULL DEFAULT 2,
	[LineDeliveryStatus] VARCHAR(20) NULL,
	[IsHighValue]  BIT NOT NULL DEFAULT 0,
	[DateLife] DATETIME NULL,
	[DateDeleted] DATETIME NULL,
	[DeletedByImport] BIT DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[LineItemId] INT NULL,
	[BagId] INT NULL,
    CONSTRAINT [PK_JobDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetail_Job] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Job] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_JobDetailStatus] FOREIGN KEY ([ShortsStatus]) REFERENCES [dbo].JobDetailStatus ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_JobDetailSource] FOREIGN KEY ([JobDetailSourceId]) REFERENCES [dbo].JobDetailSource ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_JobDetailReason] FOREIGN KEY ([JobDetailReasonId]) REFERENCES [dbo].JobDetailReason ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_DeliveryAction] FOREIGN KEY ([ShortsActionId]) REFERENCES [dbo].DeliveryAction ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_LineItem] FOREIGN KEY ([LineItemId]) REFERENCES [dbo].[LineItem] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_Bag] FOREIGN KEY ([BagId]) REFERENCES [dbo].[Bag] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
CREATE NONCLUSTERED INDEX [IDX_JobDetail_LineItemId] ON [dbo].[JobDetail] ([LineItemId]) INCLUDE ([Id],[JobId])
GO

CREATE NONCLUSTERED INDEX [JobDetails_JobId] ON [dbo].[JobDetail] (JobId DESC)
INCLUDE (OriginalDespatchQty, PHProductType, SSCCBarcode, NetPrice, LineDeliveryStatus, IsHighValue, LineItemId) 
WHERE DateDeleted IS NULL
WITH (SORT_IN_TEMPDB = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_JobDetail_SSCCBarcode] ON [dbo].[JobDetail] ([SSCCBarcode]) INCLUDE ([LineItemId])
GO
CREATE NONCLUSTERED INDEX [Idx_JobDetail_PhProductCode] ON [dbo].[JobDetail] ([PHProductCode]) INCLUDE ([Id])
GO

CREATE NONCLUSTERED INDEX [IDX_JobDetail_LineItemId_ProductDescription] ON [dbo].[JobDetail] ([LineItemId],[ProdDesc])
GO

CREATE NONCLUSTERED INDEX IDX_JobDetail_DateDeleted ON JobDetail (DateDeleted ASC) 
INCLUDE (JobId, LineItemId) WHERE [DateDeleted] IS NULL
WITH (SORT_IN_TEMPDB = ON)

GO 
CREATE NONCLUSTERED INDEX IDX_JobDetail_BagId
ON [dbo].[JobDetail] ([BagId])
INCLUDE ([JobId])

GO
