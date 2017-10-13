CREATE TABLE [dbo].[GlobalUplift](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BranchId] [int] NOT NULL,
	[PHAccount] [varchar](40) NOT NULL,
	[CsfReference] [varchar](50) NULL,
	[PHProductCode] [varchar](60) NULL,
	[ExpectedQty] [smallint] NOT NULL,
	[CollectedQty] [smallint] NULL,
	[DeliveredQty] [smallint] NULL,
	[CustomerReference] [varchar](40) NULL,
	[StartDate] [smalldatetime] NULL,
	[EndDate] [smalldatetime] NULL,
	[DateCreated] [smalldatetime] NOT NULL DEFAULT getdate(),
	[AccountName] VARCHAR(200) NULL,
	[ContactName] VARCHAR(200) NULL,
	[ContactNumber] VARCHAR(50) NULL,
 [AddressLines] VARCHAR(1000) NULL, 
    [Postcode] VARCHAR(10) NULL, 
	[SourceFilename] VARCHAR(100) NULL,
    CONSTRAINT [PK_GlobalUplift] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_GlobalUplift_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id])
) ON [PRIMARY]


GO

CREATE UNIQUE INDEX [IX_GlobalUplift_BranchAccountReference] ON [dbo].[GlobalUplift] ([BranchId], [PHAccount], [CsfReference])
