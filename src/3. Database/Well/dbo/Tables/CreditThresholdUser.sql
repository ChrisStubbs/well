CREATE TABLE [dbo].[CreditThresholdUser]
(
	[CreditThresholdId] INT NOT NULL,
	[UserId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,

	CONSTRAINT [FK_CreditThresholdUser_CreditThreshold] FOREIGN KEY ([CreditThresholdId]) REFERENCES [dbo].[CreditThreshold] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_CreditThresholdUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT UC_User_CreditThreshold UNIQUE (CreditThresholdId,UserId)
)
