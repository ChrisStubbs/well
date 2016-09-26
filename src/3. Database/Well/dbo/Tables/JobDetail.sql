
CREATE TABLE [dbo].[JobDetail]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineNumber] INT NOT NULL,
	[PHProductCode] VARCHAR(60) NULL,
	[OriginalDespatchQty] DECIMAL(8,3)  NOT NULL,
	[DeliveredQty] DECIMAL(8,3)  NULL,
	[ProdDesc] VARCHAR(100) NOT NULL,
	[OrderedQty] DECIMAL(8,3) NOT NULL,
	[ShortQty] DECIMAL(8,3)  NOT NULL,
	[UnitMeasure] VARCHAR(50) NOT NULL,
	[PHProductType] VARCHAR(50) NULL,
	[PackSize] VARCHAR(50) NULL,
	[SingleOrOuter] VARCHAR(10) NULL,
	[SSCCBarcode] VARCHAR(50) NULL,
	[SubOuterDamageTotal] INT NULL,
	[SkuGoodsValue] FLOAT NOT NULL,
	[NetPrice] FLOAT NULL,
	[JobId] INT NOT NULL,
	[JobDetailStatusId] INT NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_JobDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetail_Job] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Job] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetail_JobDetailStatus] FOREIGN KEY ([JobDetailStatusId]) REFERENCES [dbo].JobDetailStatus ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION

)
