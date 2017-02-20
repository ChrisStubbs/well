SET IDENTITY_INSERT [DeliveryAction] ON

MERGE INTO [DeliveryAction] AS Target
USING	(VALUES	(0,'Not Defined'),
				(1,'Credit'),
				(2,'Credit And Reorder'),
				(3,'Replan In Roadnet'),
				(4,'Replan In Transcend'),
				(5,'Replan In The Queue'),
				(6,'Reject'), 
				(7, 'Close'))
AS Source ([Id],[Description])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description]);

SET IDENTITY_INSERT [DeliveryAction] OFF