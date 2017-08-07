MERGE INTO [CreditThreshold] AS Target
USING	(VALUES	(1,0,'Level 1','deployment',GETDATE(),'deployment',GETDATE()),
				(2,0,'Level 2','deployment',GETDATE(),'deployment',GETDATE()),
				(3,0,'Level 3','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Level],[Threshold],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	ON Target.[Level] = Source.[Level]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Level],[Threshold],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate])
	VALUES ([Level],[Threshold],[Description],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate]);