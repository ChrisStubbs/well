SET IDENTITY_INSERT [RouteStatus] ON

MERGE INTO [RouteStatus] AS Target
USING	(VALUES	(1,'NDEPA','Not Departed','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'INPRO','In Progress','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'COMPL','Complete','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [RouteStatus] OFF