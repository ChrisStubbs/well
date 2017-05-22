SET IDENTITY_INSERT [JobDetailSource] ON

MERGE INTO [JobDetailSource] AS Target
USING	(VALUES	(0,'NotDefined'),
				(1,'Input'),
				(2,'Assembler'),
				(3,'Checker'),
				(4,'Packer'),
				(5,'Confirming'),
				(6,'Delivery'),
				(7,'RepTelesales'),
				(8,'ProductFault'),
				(9,'Customer')
		)
AS Source ([Id],[Description])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description]);

SET IDENTITY_INSERT [JobDetailSource] OFF