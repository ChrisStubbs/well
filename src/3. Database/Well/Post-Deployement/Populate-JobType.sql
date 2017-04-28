SET IDENTITY_INSERT [JobType] ON

MERGE INTO [JobType] AS Target
USING	(VALUES	(1,'DEL-TOB','Tobacco','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'DEL-AMB','Ambient','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'DEL-ALC','Alchohol','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'DEL-CHL','Chilled','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'DEL-FRZ','Frozen','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'DEL-DOC','Documents','Document delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'UPL-SAN','Uplift – Sandwiches','Uplift','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'UPL-GLO','Uplift – Global','Uplift','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'UPL-ASS','Uplift – Scheduled Asset Uplift','Uplift','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'NotDef','Not Defined','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[ActivityType],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]
WHEN MATCHED 
	AND (Target.[Description] != Source.[Description])
	THEN
	UPDATE SET 
			[Description]		= Source.[Description], 
			[ActivityType]      = Source.[ActivityType],
			[LastUpdatedBy]		= Source.[LastUpdatedBy], 
			[LastUpdatedDate]	= Source.[LastUpdatedDate]	
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[ActivityType],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[ActivityType],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);


SET IDENTITY_INSERT [JobType] OFF