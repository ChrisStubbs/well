CREATE TABLE [dbo].[JobDetailByPassReasons]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[ByPassReasonsId] TINYINT NOT NULL,
	CONSTRAINT [PK_JobDetailByPassReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailByPassReasons_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailByPassReasons_ByPassReasons] FOREIGN KEY ([ByPassReasonsId]) REFERENCES [dbo].[ByPassReasons] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
