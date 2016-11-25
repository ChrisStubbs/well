SET IDENTITY_INSERT [RoutePerformanceStatus] ON

MERGE INTO [RoutePerformanceStatus] AS Target
USING	(VALUES	(1,'NDEPA','Not Departed','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'EARLY','Early','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'LATE','Late','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'ONTIM','On Time','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'OUTOF','Out of Sequence','deployment',GETDATE(),'deployment',GETDATE()),
				(0,'NOTDEF','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [RoutePerformanceStatus] OFF