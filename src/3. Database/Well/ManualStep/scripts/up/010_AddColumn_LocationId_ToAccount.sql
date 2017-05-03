ALTER TABLE dbo.Account
ADD LocationId INT NULL CONSTRAINT [FK_Account_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]) 

