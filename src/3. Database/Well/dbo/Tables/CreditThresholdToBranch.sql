CREATE TABLE [dbo].[CreditThresholdToBranch]
(
	[BranchId] INT NOT NULL,
	[CreditThresholdId] INT NOT NULL
	CONSTRAINT [FK_Branch_CreditThreshold] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_CreditThreshold_Id] FOREIGN KEY ([CreditThresholdId]) REFERENCES [dbo].[CreditThreshold] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
