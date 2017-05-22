SET IDENTITY_INSERT [ThresholdLevel] ON

MERGE INTO [ThresholdLevel] AS Target
USING	(VALUES	(1,'Level 1','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Level 2','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Level 3','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ThresholdLevel] OFF