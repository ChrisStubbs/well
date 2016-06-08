SET IDENTITY_INSERT [JobPerformanceStatus] ON

MERGE INTO [JobPerformanceStatus] AS Target
USING	(VALUES	(1,'NARRI','Not Arrived','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'NDONE','Not Done','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'ABYPA','Authorised ByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'NBYPA','Non Authorised ByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'INCOM',',Incomplete','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'COMPL',',Complete','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [JobPerformanceStatus] OFF