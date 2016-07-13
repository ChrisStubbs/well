SET IDENTITY_INSERT [DamageReasons] ON


MERGE INTO [DamageReasons] AS Target
USING	(VALUES	(1,'CAR01','Dented','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'CAR02','Scuffed','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'CAR03','Smashed Screen','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'CAR04','Incomplete Product','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'CAR05','Product Marked','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'CAR06','Broken','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'CAR07','Not Repaired','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'CAR08','Wrong Colour / Product','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'CRR01','Dented','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'CRR02','Scuffed','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'CRR03','Smashed Screen','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'CRR04','Incomplete Product','deployment',GETDATE(),'deployment',GETDATE()),
				(13,'CRR05','Product Marked','deployment',GETDATE(),'deployment',GETDATE()),
				(14,'CRR06','Broken','deployment',GETDATE(),'deployment',GETDATE()),
				(15,'CRR07','Not Repaired','deployment',GETDATE(),'deployment',GETDATE()),
				(16,'CRR08','Wrong Colour / Product','deployment',GETDATE(),'deployment',GETDATE()),
				(17,'CRR09','Fixed on Site','deployment',GETDATE(),'deployment',GETDATE()),
				(18,'CRR10','Item already IFS','deployment',GETDATE(),'deployment',GETDATE()),
				(19,'DAR01','Dented','deployment',GETDATE(),'deployment',GETDATE()),
				(20,'DAR02','Scuffed','deployment',GETDATE(),'deployment',GETDATE()),
				(21,'DAR03','Smashed Screen','deployment',GETDATE(),'deployment',GETDATE()),
				(22,'DAR04','Incomplete Product','deployment',GETDATE(),'deployment',GETDATE()),
				(23,'DAR05','Product Marked','deployment',GETDATE(),'deployment',GETDATE()),
				(24,'DAR06','Broken','deployment',GETDATE(),'deployment',GETDATE()),
				(25,'DAR07','Not Repaired','deployment',GETDATE(),'deployment',GETDATE()),
				(26,'DAR08','Wrong Colour / Product','deployment',GETDATE(),'deployment',GETDATE()),
				(27,'DRR01','Dented','deployment',GETDATE(),'deployment',GETDATE()),
				(28,'DRR02','Scuffed','deployment',GETDATE(),'deployment',GETDATE()),
				(29,'DRR03','Smashed Screen','deployment',GETDATE(),'deployment',GETDATE()),
				(30,'DRR04','Incomplete Product','deployment',GETDATE(),'deployment',GETDATE()),
				(31,'DRR05','Product Marked','deployment',GETDATE(),'deployment',GETDATE()),
				(32,'DRR06','Broken','deployment',GETDATE(),'deployment',GETDATE()),
				(33,'DRR07','Not Repaired','deployment',GETDATE(),'deployment',GETDATE()),
				(34,'DRR08','Wrong Colour / Product','deployment',GETDATE(),'deployment',GETDATE()),
				(35,'DRR09','Delivered to Store','deployment',GETDATE(),'deployment',GETDATE()),
				(36,'DRR10','Unused Loan','deployment',GETDATE(),'deployment',GETDATE()),
				(37,'Notdef','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [DamageReasons] OFF