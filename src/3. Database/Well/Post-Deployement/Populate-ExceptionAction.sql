﻿SET IDENTITY_INSERT [ExceptionAction] ON

-- see EventAction enum
-- these actions correspond to transaction types in the ADAM world

MERGE INTO [ExceptionAction] AS Target
USING	(VALUES	(1,'Credit','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'CreditAndReorder','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Grn','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Pod','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'StandardUplift','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'Amendment','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'GlobalUplift','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'RecirculateDocuments','deployment',GETDATE(),'deployment',GETDATE()),
				(20,'PodTransaction','deployment',GETDATE(),'deployment',GETDATE()))

AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])

WHEN MATCHED AND Target.[Description] != Source.[Description] THEN
	UPDATE SET
	    [Description] = Source.[Description],
		[LastUpdatedBy] = Source.[LastUpdatedBy],
		[LastUpdatedDate] = Source.[LastUpdatedDate];

SET IDENTITY_INSERT [ExceptionAction] OFF