SET IDENTITY_INSERT [UserRole] ON

MERGE INTO [UserRole] AS Target
USING	(VALUES	(1,'Branch Manager','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Customer Service Manager','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Customer Service User','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [UserRole] OFF