CREATE TABLE [dbo].[SeasonalDateToBranch]
(
	[BranchId] INT NOT NULL,
	[SeasonalDateId] INT NOT NULL
	CONSTRAINT [FK_Branch_Id] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_SeasonalDate_Id] FOREIGN KEY ([SeasonalDateId]) REFERENCES [dbo].[SeasonalDate] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
