SET IDENTITY_INSERT [DamageSource] ON


MERGE INTO [DamageSource] AS Target
USING	(VALUES	(1,'PSTDIS001','The Warehouse','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'PDADIS001','Manufacturer','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'PDRDIS002','Warehouse','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'NotDef','Not Defined','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [DamageSource] OFF
