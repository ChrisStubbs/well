SET IDENTITY_INSERT [Branch] ON

MERGE INTO [Branch] AS Target
USING	(VALUES	(3,'Coventry','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Fareham','deployment',GETDATE(),'deployment',GETDATE()),
				(55,'Plymouth','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Medway','deployment',GETDATE(),'deployment',GETDATE()),
				(20,'Hemel','deployment',GETDATE(),'deployment',GETDATE()),
				(59,'Bristol','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'Dunfermline','deployment',GETDATE(),'deployment',GETDATE()),
				(22, 'Birtley','deployment',GETDATE(),'deployment',GETDATE()),
				(42,'Brandon','deployment',GETDATE(),'deployment',GETDATE()),
				(33,'Belfast','deployment',GETDATE(),'deployment',GETDATE()),
				(14,'Leads','deployment',GETDATE(),'deployment',GETDATE()),
				(82,'Haydock','deployment',GETDATE(),'deployment',GETDATE()),
				(49,'Leads 2','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Name],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Name],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Name],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [Branch] OFF