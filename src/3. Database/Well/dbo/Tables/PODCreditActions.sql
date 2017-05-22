CREATE TABLE [dbo].[PODCreditActions]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CreditActions] VARCHAR(30) NOT NULL,
	[PDACreditReasonId] INT NOT NULL,
	[PODCreditReasonId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_PODCreditActions] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_PODCreditActions_PDACreditReasons] FOREIGN KEY ([PDACreditReasonId]) REFERENCES [dbo].PDACreditReasons ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_PODCreditActions_PODCreditReasons] FOREIGN KEY ([PODCreditReasonId]) REFERENCES [dbo].PODCreditReasons ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
