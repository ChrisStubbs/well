CREATE TABLE [dbo].[LineItem]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineNumber] SMALLINT NULL,
	[ProductCode] VARCHAR(60) NOT NULL,
	[ProductDescription] VARCHAR(100) NOT NULL,
	[AmendedDeliveryQuantity] INT NULL,
	[AmendedShortQuantity] INT NULL,
	[OriginalShortQuantity] INT NULL,
	[BagId] INT NULL,
	[ActivityId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] SMALLDATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] SMALLDATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    [DateDeleted] SMALLDATETIME NULL, 
	[DeletedByImport] BIT DEFAULT 0,
	[JobId] INT NULL,
    CONSTRAINT [PK_LineItem] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItem_Bag] FOREIGN KEY ([BagId]) REFERENCES [dbo].[Bag] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItem_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItem_Job] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Job] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)

GO
CREATE NONCLUSTERED INDEX [IDX_LineItem_ActivityId] ON [dbo].[LineItem] ([ActivityId]) INCLUDE ([Id],[ProductCode],[ProductDescription])
GO

CREATE NONCLUSTERED INDEX IDX_LineItem_DateDeleted ON LineItem (DateDeleted ASC)
INCLUDE (ActivityId,JobId) 
WHERE DateDeleted IS NULL
WITH (SORT_IN_TEMPDB = ON) 

GO 

CREATE NONCLUSTERED INDEX IDx_LineItem_JobId
ON [dbo].[LineItem] ([JobId])
INCLUDE ([Id])

