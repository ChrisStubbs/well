SET IDENTITY_INSERT [ReasonCategory] ON

MERGE INTO [ReasonCategory] AS Target
USING	(VALUES	(1,'Auth','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'DA','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'DR','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ReasonCategory] OFF