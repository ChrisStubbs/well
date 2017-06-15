SET IDENTITY_INSERT ResolutionStatus ON

MERGE INTO ResolutionStatus AS Target
USING	(VALUES	(1, 'Imported'),
				(2, 'Driver Completed'),
				(4, 'Action Required'),
				(8, 'Pending Submission'),
				(16, 'Pending Approval'),
				(32, 'Credited'),
				(64, 'Resolved'),
				(128, 'Closed')
		)
AS Source (Id, [Description])
	ON Target.Id = Source.Id

WHEN NOT MATCHED BY TARGET THEN
	INSERT (Id, [Description])
	VALUES (Id, [Description]);

SET IDENTITY_INSERT ResolutionStatus OFF