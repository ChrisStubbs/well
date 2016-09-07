CREATE TABLE [dbo].[JobDetailDamage]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[Qty] DECIMAL(7,3) NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[DamageReasonsId] TINYINT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_JobDetailDamageReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailDamageReasons_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailDamageReasons_DamageReasons] FOREIGN KEY ([DamageReasonsId]) REFERENCES [dbo].[DamageReasons] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
