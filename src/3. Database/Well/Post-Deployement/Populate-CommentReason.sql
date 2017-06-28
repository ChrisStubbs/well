SET IDENTITY_INSERT [CommentReason] ON
MERGE INTO [CommentReason] AS Target
USING	(VALUES	(1,'Amendment following customer contact','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Amendment following an internal investigation','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Correction following incorrect data entry','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Entry following a manual – paper based delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Ammendment following a bulk update','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
WHEN MATCHED 
	AND (Target.Description != Source.Description)
	THEN
	UPDATE SET 
			[Description]	= Source.[Description], 
			[UpdatedBy]		= Source.[UpdatedBy], 
			[DateUpdated]	= Source.[DateUpdated]	
	
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CommentReason] OFF