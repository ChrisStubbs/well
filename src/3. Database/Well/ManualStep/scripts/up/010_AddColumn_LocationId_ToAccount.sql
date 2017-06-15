IF COL_LENGTH('dbo.Account', 'LocationId') IS NULL
BEGIN
	ALTER TABLE dbo.Account
	ADD LocationId INT NULL 
	CONSTRAINT [FK_Account_Location] 
	FOREIGN KEY ([LocationId]) 
	REFERENCES [dbo].[Location] ([Id]) 
END
