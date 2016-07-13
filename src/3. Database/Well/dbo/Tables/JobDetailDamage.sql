CREATE TABLE [dbo].[JobDetailDamage]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[Qty] INT NOT NULL,
	[DamageReasonsId] TINYINT NOT NULL,
	[DamageReasonCategoryId] TINYINT NOT NULL,
	CONSTRAINT [PK_JobDetailDamageReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailDamageReasons_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailDamageReasons_DamageReasons] FOREIGN KEY ([DamageReasonsId]) REFERENCES [dbo].[DamageReasons] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailDamageReasons_DamageReasonCategory] FOREIGN KEY ([DamageReasonCategoryId]) REFERENCES [dbo].[ReasonCategory] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
