SET IDENTITY_INSERT [DeliveryAction] ON

MERGE INTO [DeliveryAction] AS Target
USING	(VALUES	(0,'Not Defined'),
				(1,'Credit'),
				(2,'Close'),
				(3,'Pod')
			)
AS Source ([Id],[Description])
	ON Target.[Id] = Source.[Id]
WHEN  MATCHED AND Target.[Description] != Source.[Description] THEN
	UPDATE 
	SET Target.[Description] = Source.[Description]
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description])
WHEN NOT MATCHED BY SOURCE THEN
  DELETE;
;

SET IDENTITY_INSERT [DeliveryAction] OFF
