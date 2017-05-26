SET IDENTITY_INSERT [WellStatus] ON

MERGE INTO [WellStatus] AS Target
USING	(VALUES	(0,'Not defined', 'Not defined','deployment',GETDATE(),'deployment',GETDATE()),
				(1,'Planned', 'Planned','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Invoiced', 'Invoiced','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Complete', 'Complete','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Bypassed', 'Bypassed','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'In Progress', 'Route in progress','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[DisplayName],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [WellStatus] OFF
