CREATE TABLE [dbo].[JobDetailDamageReasons]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[DamageReasonsId] TINYINT NOT NULL,
	CONSTRAINT [PK_JobDetailDamageReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailDamageReasons_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailDamageReasons_DamageReasons] FOREIGN KEY ([DamageReasonsId]) REFERENCES [dbo].[DamageReasons] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
