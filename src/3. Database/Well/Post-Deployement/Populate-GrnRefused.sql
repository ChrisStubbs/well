SET IDENTITY_INSERT [GrnRefused] ON

MERGE INTO [GrnRefused] AS Target
USING	(VALUES	(1,'Inc','Incorrect','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'NotNd','NotNeeded','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [GrnRefused] OFF
