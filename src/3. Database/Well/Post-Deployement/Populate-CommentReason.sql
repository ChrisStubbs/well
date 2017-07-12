SET IDENTITY_INSERT [CommentReason] ON
MERGE INTO [CommentReason] AS Target
USING	(VALUES	(1,0,'Amendment following customer contact','deployment',GETDATE(),'deployment',GETDATE()),
				(2,0,'Amendment following an internal investigation','deployment',GETDATE(),'deployment',GETDATE()),
				(3,0,'Correction following incorrect data entry','deployment',GETDATE(),'deployment',GETDATE()),
				(4,0,'Entry following a manual – paper based delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(5,0,'Amendment following a bulk update','deployment',GETDATE(),'deployment',GETDATE()),
				(6,1,'Quantity automatically set','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[IsDefault],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
WHEN MATCHED 
	AND (Target.Description != Source.Description)
	THEN
	UPDATE SET 
			[IsDefault]		= Source.[IsDefault],
			[Description]	= Source.[Description], 
			[UpdatedBy]		= Source.[UpdatedBy], 
			[DateUpdated]	= Source.[DateUpdated]	
	
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[IsDefault],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[IsDefault],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CommentReason] OFF