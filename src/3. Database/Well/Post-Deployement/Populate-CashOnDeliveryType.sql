SET IDENTITY_INSERT [CashOnDeliveryType] ON

MERGE INTO [CashOnDeliveryType] AS Target
USING	(VALUES	(1,'CashOnly','Strictly cash on delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'CCC','Cash, Card or Cheque','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[Code],[Description],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [CashOnDeliveryType] OFF
