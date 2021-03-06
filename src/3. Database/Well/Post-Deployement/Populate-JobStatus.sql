﻿SET IDENTITY_INSERT [JobStatus] ON

MERGE INTO [JobStatus] AS Target
USING	(VALUES	(1,'Awaiting Invoice'),
				(2,'InComplete'),
				(3,'Clean'),
				(4,'Exception'),
				(5,'Resolved'),
				(6, 'Document Delivery'),
				(7, 'Completed On Paper'),
				(8, 'Bypassed'),
				(9, 'Replanned')
		)
AS Source ([Id],[Description])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description]);

SET IDENTITY_INSERT [JobStatus] OFF
