SET IDENTITY_INSERT [CommodityType] ON

MERGE INTO [CommodityType] AS Target
USING	(VALUES	(1,'A','Alcohol Outers','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'B','Alcohol Singles','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'C','Confectionary','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'F','Frozen','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'G','Chilled','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'I','Impulsive','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'J','Central','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'K','Central Singles','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'O','Confectionary Singles','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'P','Phone Cards','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'T','Tobabcco','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'U','Hub Tobabcco','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CommodityType] OFF
