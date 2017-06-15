SET IDENTITY_INSERT ResolutionStatus ON

delete ResolutionStatus

MERGE INTO ResolutionStatus AS Target
USING	(VALUES	(1, 'Imported'),
				(2, 'Driver Completed'),
				(3, 'Action Required'),
				(4, 'Pending Submission'),
				(5, 'Pending Approval'),
				(6, 'Approved'),
				(7, 'Credited'),
				(8, 'Resolved'),
				(9, 'Closed')
		)
AS Source (Id, [Description])
	ON Target.Id = Source.Id

WHEN NOT MATCHED BY TARGET THEN
	INSERT (Id, [Description])
	VALUES (Id, [Description]);

SET IDENTITY_INSERT ResolutionStatus OFF