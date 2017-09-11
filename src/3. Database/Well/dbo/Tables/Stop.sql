CREATE TABLE [dbo].[Stop]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TransportOrderReference] VARCHAR(50) NOT NULL,
	[PlannedStopNumber] VARCHAR(4) NOT NULL,
	[RouteHeaderCode] VARCHAR(10)  NULL,
	[RouteHeaderId] INT NOT NULL,
	[DropId] VARCHAR(2) NULL,
	[Previously] VARCHAR(500) NULL,
	[LocationId] VARCHAR(20) NULL,
	[DeliveryDate] DATETIME NULL,
	[ShellActionIndicator] varchar(100) NULL,
	[AllowOvers] BIT NULL,
	[CustUnatt] BIT NULL,
	[PHUnatt] BIT NULL,
	[StopStatusCode] VARCHAR(50) NULL,
	[StopStatusDescription] VARCHAR(255) NULL,
	[PerformanceStatusCode] VARCHAR(50) NULL,
	[PerformanceStatusDescription] VARCHAR(255) NULL,
	[Reason] VARCHAR(255) NULL,
    [DateDeleted] DATETIME NULL,
	[DeletedByImport] BIT DEFAULT 0,
	[ActualPaymentCash] DECIMAL(7,2) NULL,
	[ActualPaymentCheque] DECIMAL(7,2) NULL,
	[ActualPaymentCard] DECIMAL(7,2) NULL,
	[AccountBalance] DECIMAL (7,2) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[Location_Id] INT NULL,
	[WellStatus] TINYINT NULL,
    CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Stops_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteHeader] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stop_Location] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[Location] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stop_WellStatus] FOREIGN KEY ([WellStatus]) REFERENCES [dbo].[WellStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
CREATE NONCLUSTERED INDEX [IDX_Stop_RouteHeaderId] ON [dbo].[Stop] ([RouteHeaderId]) INCLUDE ([Id])
GO
CREATE NONCLUSTERED INDEX [idx_Stop_LocationId] ON [dbo].[Stop] ([Location_Id])
GO
CREATE NONCLUSTERED INDEX Idx_Stop_TransportOrderReference ON [dbo].[Stop] ([TransportOrderReference]) INCLUDE ([Id])
