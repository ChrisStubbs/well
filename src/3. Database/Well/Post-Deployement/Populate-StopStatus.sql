SET IDENTITY_INSERT [StopStatus] ON

MERGE INTO [StopStatus] AS Target
USING	(VALUES	(1,'NARRI','Not Arrived','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'ARRIV','Arrived','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'DEPAR','Departed','deployment',GETDATE(),'deployment',GETDATE()),
				(0,'NOTDEF','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [StopStatus] OFF