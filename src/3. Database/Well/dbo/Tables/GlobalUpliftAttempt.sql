CREATE TABLE [dbo].[GlobalUpliftAttempt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GlobalUpliftId] [int] NOT NULL,
	[DateAttempted] [smalldatetime] NOT NULL,
	[DriverName] [varchar](50) NOT NULL,
	[RouteNumber] [varchar](12) NOT NULL,
	[PlannedStopNumber] [varchar](4) NOT NULL,
	[JobStatusId] [tinyint] NOT NULL,
	[CollectedQty] [smallint] NULL,
	[SourceFilename] VARCHAR(100) NULL,
    CONSTRAINT [PK_GlobalUpliftAttempt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_GlobalUpliftAttempt_ToGlobalUplift] FOREIGN KEY ([GlobalUpliftId]) REFERENCES [GlobalUplift]([Id]), 
    CONSTRAINT [FK_GlobalUpliftAttempt_ToJobStatus] FOREIGN KEY ([JobStatusId]) REFERENCES [JobStatus]([Id])
) ON [PRIMARY]

GO

CREATE INDEX [IX_GlobalUpliftAttempt_GlobalUpliftIdDate] ON [dbo].[GlobalUpliftAttempt] ([GlobalUpliftId], [DateAttempted])
