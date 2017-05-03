SET IDENTITY_INSERT [ActivityType] ON

MERGE INTO [ActivityType] AS Target
USING	(VALUES	(1,'Invoice','Invoice','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Uplift','Goods to be collected','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Documents','Document delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Not defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ActivityType] OFF