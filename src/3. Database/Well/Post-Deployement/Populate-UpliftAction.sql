SET IDENTITY_INSERT [UpliftAction] ON

MERGE INTO [UpliftAction] AS Target
USING	(VALUES	(0,'Uplift and credit','deployment',GETDATE(),'deployment',GETDATE()),
				(1,'Credit no uplift','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Uplift no credit','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'No uplift no credit','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
WHEN MATCHED AND Target.Description != Source.Description THEN
	UPDATE SET
		[Description]		= Source.[Description], 
		LastUpdatedBy	= Source.LastUpdatedBy, 
		LastUpdatedDate	= Source.LastUpdatedDate;	

SET IDENTITY_INSERT [UpliftAction] OFF