SET IDENTITY_INSERT [DeliveryAction] ON

MERGE INTO [DeliveryAction] AS Target
USING	(VALUES	(0,'Not Defined'),
				(1,'Credit'),
				(3,'Reject'), 
				(4, 'Close'))
AS Source ([Id],[Description])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description]);

SET IDENTITY_INSERT [DeliveryAction] OFF