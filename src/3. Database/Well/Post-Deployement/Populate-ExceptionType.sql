SET IDENTITY_INSERT [ExceptionType] ON

MERGE INTO [ExceptionType] AS Target
USING	(VALUES	(0,'Not Defined','Not Defined','deployment',GETDATE(),'deployment',GETDATE()),
				(1,'Short','Short delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Bypass','Bypassed','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Damage','Damaged delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Uplifted','Successful uplift','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ExceptionType] OFF