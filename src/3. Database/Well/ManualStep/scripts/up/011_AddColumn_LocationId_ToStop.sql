﻿IF COL_LENGTH('dbo.Stop', 'Location_Id') IS NULL
BEGIN
	ALTER TABLE dbo.[Stop]
	ADD Location_Id INT NULL 
	CONSTRAINT [FK_Stop_Location] 
	FOREIGN KEY ([Location_Id]) 
	REFERENCES [dbo].[Location] ([Id]) 
END
