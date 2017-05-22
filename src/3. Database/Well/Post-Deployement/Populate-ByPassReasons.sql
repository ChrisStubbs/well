SET IDENTITY_INSERT [ByPassReasons] ON

MERGE INTO [ByPassReasons] AS Target
USING	(VALUES	(1,'-16','No Adult Signature','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'CIB','Instructions Required','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'F','Product Faulty','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'ID','Incorrect Details','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'IP','Insecure Premises','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'LR','Loan Required','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'NBI','Customer not at Home','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'PC','Job Part Complete','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'Z','Customer Cancelled on Pre-call','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'Z1','Store Cancelled - file not complete','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'Z2','Store Cancelled - Stock not ready','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'Z3','No spur plate on pre-call','deployment',GETDATE(),'deployment',GETDATE()),
				(13,'Notdef','Not Defined','deployment',GETDATE(),'deployment',GETDATE()),
				(14,'BypassedStop','Bypassed Stop','deployment',GETDATE(),'deployment',GETDATE())

		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [ByPassReasons] OFF