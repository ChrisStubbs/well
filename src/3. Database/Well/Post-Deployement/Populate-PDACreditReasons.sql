SET IDENTITY_INSERT [PDACreditReasons] ON

MERGE INTO [PDACreditReasons] AS Target
USING	(VALUES	(1,'Damaged','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Short','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'Rejected','deployment',GETDATE(),'deployment',GETDATE())

		)
AS Source ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [PDACreditReasons] OFF