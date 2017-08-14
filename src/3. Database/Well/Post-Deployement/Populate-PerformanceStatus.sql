SET IDENTITY_INSERT [PerformanceStatus] ON

MERGE INTO [PerformanceStatus] AS Target
USING	(VALUES	(1,'NARRI','Not Arrived','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'NDONE','Not Done','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'ABYPA','Authorised ByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'NBYPA','Non Authorised ByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'INCOM','Incomplete','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'COMPL','Complete','deployment',GETDATE(),'deployment',GETDATE()),
				(0,'NOTDEF','Not Defined','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'RESOLVED','Resolved','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'PENDING','Pending Authorisation','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'SUBMITTED','Submitted','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'WBYPA','Well ByPass','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [PerformanceStatus] OFF