SET IDENTITY_INSERT ResolutionStatus ON

MERGE INTO ResolutionStatus AS Target
USING	(VALUES	(1,			'Imported'),
				(2,			'Driver Completed'),
				(4,			'Action Required'),
				(8,			'Pending Submission'),
				(16,		'Pending Approval'),
				(32,		'Approved'),
				(64,		'Credited'),
				(128,       'Resolved'),
				(256 | 2,   'Closed - Driver Completed'),
				(256 | 64,  'Closed - Credited'),
				(256 | 128, 'Closed - Resolved'),
				(512,		'Completed by Well')
		)
AS Source (Id, [Description])
	ON Target.Id = Source.Id

WHEN NOT MATCHED BY TARGET THEN
	INSERT (Id, [Description])
	VALUES (Id, [Description]);

SET IDENTITY_INSERT ResolutionStatus OFF