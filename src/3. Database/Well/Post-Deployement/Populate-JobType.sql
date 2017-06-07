SET IDENTITY_INSERT JobType ON

DELETE JobType

MERGE INTO JobType AS Target
USING	(VALUES	(1,'DEL-TOB','Tobacco', 'Tob', dbo.ActivityType_Invoice(),'deployment',GETDATE(),'deployment',GETDATE()),
				(2,'DEL-AMB','Ambient', 'Amb', dbo.ActivityType_Invoice(),'deployment',GETDATE(),'deployment',GETDATE()),
				(3,'DEL-ALC','Alchohol', 'Alc', dbo.ActivityType_Invoice(),'deployment',GETDATE(),'deployment',GETDATE()),
				(4,'DEL-CHL','Chilled', 'Chi', dbo.ActivityType_Invoice(),'deployment',GETDATE(),'deployment',GETDATE()),
				(5,'DEL-FRZ','Frozen', 'Frz', dbo.ActivityType_Invoice(),'deployment',GETDATE(),'deployment',GETDATE()),
				(6,'DEL-DOC','Documents', 'Doc', dbo.ActivityType_Documents(),'deployment',GETDATE(),'deployment',GETDATE()),
				(7,'UPL-SAN','Uplift – Sandwiches', 'USD', dbo.ActivityType_Uplift(),'deployment',GETDATE(),'deployment',GETDATE()),
				(8,'UPL-GLO','Uplift – Global', 'UGL', dbo.ActivityType_Uplift(),'deployment',GETDATE(),'deployment',GETDATE()),
				(9,'UPL-ASS','Uplift – Scheduled Asset Uplift', 'USA', dbo.ActivityType_Uplift(),'deployment',GETDATE(),'deployment',GETDATE()),
				(10,'UPL-STD','Uplift – Standard', 'UST', dbo.ActivityType_Uplift(),'deployment',GETDATE(),'deployment',GETDATE()),
				(11,'NotDef','Not Defined', '', dbo.ActivityType_NotDefined(),'deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source (Id, Code, Description, Abbreviation, ActivityTypeId, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	ON Target.Id = Source.Id
WHEN MATCHED 
	AND (Target.Description != Source.Description)
	THEN
	UPDATE SET 
			Code			= Source.Code,
			Description		= Source.Description, 
			Abbreviation	= Source.Abbreviation,
			ActivityTypeId  = Source.ActivityTypeId,
			LastUpdatedBy	= Source.LastUpdatedBy, 
			LastUpdatedDate	= Source.LastUpdatedDate	
WHEN NOT MATCHED BY TARGET THEN
	INSERT (Id, Code, Description, Abbreviation, ActivityTypeId, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	VALUES (Id, Code, Description, Abbreviation, ActivityTypeId, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate);

ALTER TABLE JobType
ALTER COLUMN Abbreviation Char(3) NOT NULL

SET IDENTITY_INSERT JobType OFF