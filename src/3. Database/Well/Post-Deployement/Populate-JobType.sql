SET IDENTITY_INSERT [JobType] ON

MERGE INTO [JobType] AS Target
USING	(VALUES	(1,'DEL-TOB','Tobacco','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'DEL-AMB','Ambient','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'DEL-ALC','Alchohol','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'DEL-CHL','Chilled','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'DEL-FRZ','Frozen','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'DEL-DOC','Documents','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'UPL-SAN','IQDOCSTYPE','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'UPL-GLO','IQDOCSTYPE','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'UPL-ASS','IQDOCSTYPE','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'NotDef','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]
WHEN MATCHED 
	AND (Target.[Description] != Source.[Description])
	THEN
	UPDATE SET 
			[Description]		= Source.[Description], 
			[LastUpdatedBy]		= Source.[LastUpdatedBy], 
			[LastUpdatedDate]	= Source.[LastUpdatedDate]	
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);


SET IDENTITY_INSERT [JobType] OFF