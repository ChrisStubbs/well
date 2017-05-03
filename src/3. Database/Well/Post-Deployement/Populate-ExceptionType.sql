SET IDENTITY_INSERT [ExceptionType] ON

MERGE INTO [ExceptionType] AS Target
USING	(VALUES	(1,'Short','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Bypass','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Damage','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ExceptionType] OFF