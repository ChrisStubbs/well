SET IDENTITY_INSERT [AccountType] ON

MERGE INTO [AccountType] AS Target
USING	(VALUES	(1,'CUST','Customer','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'OTHER','Other','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'OUT','Outbase','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'STORE','Store','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'SUPP','Supplier','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [AccountType] OFF