SET IDENTITY_INSERT [CommentReason] ON

MERGE INTO [CommentReason] AS Target
USING	(VALUES	(1,'This is a comment reason','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'This is also a comment reason','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CommentReason] OFF
