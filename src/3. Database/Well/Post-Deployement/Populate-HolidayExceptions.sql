SET IDENTITY_INSERT [HolidayExceptions] ON

MERGE INTO [HolidayExceptions] AS Target
USING	(VALUES	(1,'2016-08-29','Summer bank holiday','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'2016-12-25','Christmas Day','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'2016-12-26','Boxing Day','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'2016-12-27','Christmas Day (substitute bank holiday)','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'2017-01-02','New Year’s Day (substitute bank holiday)','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'2017-04-14','Good Friday','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'2017-04-17','Easter Monday','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'2017-05-01','Early May Bank Holiday','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'2017-05-29','Spring Bank Holiday','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'2017-08-28','Summer Bank Holiday','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'2017-12-25','Christmas Day','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'2017-12-26','Boxing Day','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[ExceptionDate],[Exception],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[ExceptionDate],[Exception],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[ExceptionDate],[Exception],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [HolidayExceptions] OFF
