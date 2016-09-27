SET IDENTITY_INSERT [ActionStatus] ON

MERGE INTO [ActionStatus] AS Target
USING	(VALUES	(1,'Draft','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Submitted','deployment',GETDATE(),'deployment',GETDATE()))
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ActionStatus] OFF