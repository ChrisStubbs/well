SET IDENTITY_INSERT [Branch] ON

MERGE INTO [Branch] AS Target
USING	(VALUES	(3,'Coventry','cov','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Fareham','far','deployment',GETDATE(),'deployment',GETDATE()),
				(55,'Plymouth','ply','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Medway','med','deployment',GETDATE(),'deployment',GETDATE()),
				(20,'Hemel','hem','deployment',GETDATE(),'deployment',GETDATE()),
				(59,'Bristol','bri','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'Dunfermline','dun','deployment',GETDATE(),'deployment',GETDATE()),
				(22, 'Birtley','bir','deployment',GETDATE(),'deployment',GETDATE()),
				(42,'Brandon','bra','deployment',GETDATE(),'deployment',GETDATE()),
				(33,'Belfast','bel','deployment',GETDATE(),'deployment',GETDATE()),
				(14,'Leeds','lee','deployment',GETDATE(),'deployment',GETDATE()),
				(82,'Haydock','hay','deployment',GETDATE(),'deployment',GETDATE()),
				(99,'Not Defined','ndf','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Name],[TranscendMapping],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Name],[TranscendMapping],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Name],[TranscendMapping],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [Branch] OFF