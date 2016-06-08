SET IDENTITY_INSERT [JobType] ON

MERGE INTO [JobType] AS Target
USING	(VALUES	(1,'AD-HOC','Ad Hoc Collection','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'COL','Collection','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'D-COL','Store Delivery Collection','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'DEL','Delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'HMC','House Move Collection','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'HMD','House Move Delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'LC','Loan Collect','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'LOAN','Loan Delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'RE DEL','Re Delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'SC','Service Call','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'SD','Showroom Delivery','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'SR','Service Return','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Id],[Code],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);

SET IDENTITY_INSERT [JobType] OFF