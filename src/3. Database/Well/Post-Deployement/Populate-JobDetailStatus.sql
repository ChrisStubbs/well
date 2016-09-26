SET IDENTITY_INSERT [JobDetailStatus] ON

MERGE INTO [JobDetailStatus] AS Target
USING	(VALUES	(1,'Resolved','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'Unresolved','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'On Hold','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'Awaiting Invoice Number','deployment',GETDATE(),'deployment',GETDATE())

		)
AS Source ([Id],[Status],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Status],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Status],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [JobDetailStatus] OFF
