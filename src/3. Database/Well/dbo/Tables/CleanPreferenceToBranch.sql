CREATE TABLE [dbo].[CleanPreferenceToBranch]
(
	[BranchId] INT NOT NULL,
	[CleanPreferenceId] INT NOT NULL
	CONSTRAINT [FK_Branch_CleanPreference] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_CleanPreference_Id] FOREIGN KEY ([CleanPreferenceId]) REFERENCES [dbo].[CleanPreference] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
