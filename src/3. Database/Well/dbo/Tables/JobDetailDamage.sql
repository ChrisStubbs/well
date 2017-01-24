CREATE TABLE [dbo].[JobDetailDamage]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[Qty] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,	
	JobDetailSource TINYINT NULL CONSTRAINT FK_JobDetailDamage_JobDetailSource REFERENCES [dbo].JobDetailSource (Id),
	JobDetailReason TINYINT NULL CONSTRAINT FK_JobDetailDamage_JobDetailReason REFERENCES [dbo].[JobDetailReason] (Id),
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_JobDetailDamageReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailDamageReasons_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
