CREATE TABLE [dbo].[Stop]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[PlannedStopNumber] NVARCHAR(4) NOT NULL,
	[PlannedArriveTime] TIME NOT NULL,
	[PlannedDepartTime] TIME NOT NULL,
	[RouteHeaderId] INT NOT NULL,
	[DropId] NVARCHAR(2) NOT NULL,
	[LocatiodId] NVARCHAR(20) NOT NULL,
	[DeliveryDate] DATETIME NOT NULL,
	[SpecialInstructions] NVARCHAR(100) NOT NULL,
	[StartWindow] TIME NOT NULL,
	[EndWindow] TIME NOT NULL,
	[TextField1] nvarchar(100) NULL,
	[TextField2] nvarchar(100) NULL,
	[TextField3] nvarchar(100) NULL,
	[TextField4] nvarchar(100) NULL,
	[BypassReasonId] TINYINT NULL,
	CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Stops_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteHeader] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stops_BypassReason] FOREIGN KEY ([BypassReasonId]) REFERENCES [dbo].[ByPassReasons] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
