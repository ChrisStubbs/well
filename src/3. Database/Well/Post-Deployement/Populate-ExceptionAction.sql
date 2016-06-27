SET IDENTITY_INSERT [ExceptionAction] ON

MERGE INTO [ExceptionAction] AS Target
USING	(VALUES	(1,'Credit','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'CreditAndReorder','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'ReplanInRoadnet','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'ReplanInTranscend','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'ReplanInTheQueue','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'Reject','deployment',GETDATE(),'deployment',GETDATE()))
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ExceptionAction] OFF