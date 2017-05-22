SET IDENTITY_INSERT [CSFRejection] ON

MERGE INTO [CSFRejection] AS Target
USING	(VALUES	(1,'Unreturned Goods','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Damaged Returns','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'UnPaid','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Not Signed Short','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'Products Found','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'Duplicate Claim','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[RejectionReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[RejectionReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[RejectionReason],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CSFRejection] OFF
