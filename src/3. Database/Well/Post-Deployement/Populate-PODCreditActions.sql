SET IDENTITY_INSERT [PODCreditActions] ON

MERGE INTO [PODCreditActions] AS Target
USING	(VALUES	(1,'[R]Reduced', 1, 1, 'deployment',GETDATE(),'deployment',GETDATE()),
				(2,'[R]educed quantity', 2, 2, 'deployment',GETDATE(),'deployment',GETDATE()),
				(3,'[N]ot delivered', 2, 2, 'deployment',GETDATE(),'deployment',GETDATE()),
				(4,'[N]ot delivered', 3, 3, 'deployment',GETDATE(),'deployment',GETDATE())

		)
AS Source ([Id],[CreditActions],[PDACreditReasonId], [PODCreditReasonId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[CreditActions],[PDACreditReasonId],[PODCreditReasonId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[CreditActions],[PDACreditReasonId],[PODCreditReasonId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [PODCreditActions] OFF
