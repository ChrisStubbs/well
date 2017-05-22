CREATE TABLE [dbo].[Reason]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[ReasonCode] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(100) NOT NULL,
	[JobDetailDamageId] INT NOT NULL,

	CONSTRAINT [PK_Reason] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [PK_ReasonJobDetailDamage] FOREIGN KEY ([JobDetailDamageId]) REFERENCES [dbo].[JobDetailDamage] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
	
)
