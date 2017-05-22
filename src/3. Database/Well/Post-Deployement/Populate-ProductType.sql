SET IDENTITY_INSERT [ProductType] ON

MERGE INTO [ProductType] AS Target
USING	(VALUES	(1,'Tob','Tobacco','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Chld','Chilled','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Frzn','Frozen','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Alc','Alcohol','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Amb','Ambient','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [ProductType] OFF
