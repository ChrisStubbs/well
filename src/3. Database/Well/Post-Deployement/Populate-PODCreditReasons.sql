SET IDENTITY_INSERT [PODCreditReasons] ON

MERGE INTO [PODCreditReasons] AS Target
USING	(VALUES	(1,'[DA] Damaged','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'[MS] Delivery Failure','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'[RF] Refused','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[CreditReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [PODCreditReasons] OFF