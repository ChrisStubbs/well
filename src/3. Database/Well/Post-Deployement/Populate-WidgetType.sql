SET IDENTITY_INSERT [WidgetType] ON

MERGE INTO [WidgetType] AS Target
USING	(VALUES	(1,'Exceptions','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Assigned','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Outstanding','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Notifications','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [WidgetType] OFF
