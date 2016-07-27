SET IDENTITY_INSERT [RouteAttributeExceptions] ON

MERGE INTO [RouteAttributeExceptions] AS Target
USING	(VALUES	(1,'RouteHeader','RouteStatusCode','deployment',GETDATE(),'deployment',GETDATE()),
				(2,'RouteHeader','RoutePerformanceStatusCode','deployment',GETDATE(),'deployment',GETDATE()),
				(3,'RouteHeader','LastRouteUpdate','deployment',GETDATE(),'deployment',GETDATE()),
				(4,'RouteHeader','AuthByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(5,'RouteHeader','NonAuthByPass','deployment',GETDATE(),'deployment',GETDATE()),
				(6,'RouteHeader','ShortDeliveries','deployment',GETDATE(),'deployment',GETDATE()),
				(7,'RouteHeader','DamagesRejected','deployment',GETDATE(),'deployment',GETDATE()),
				(8,'RouteHeader','DamagesAccepted','deployment',GETDATE(),'deployment',GETDATE()),
				(9,'RouteHeader','NotRequired','deployment',GETDATE(),'deployment',GETDATE()),
				(10,'RouteHeader','Depot','deployment',GETDATE(),'deployment',GETDATE()),
				(11,'RouteHeader','ActualStopsCompleted','deployment',GETDATE(),'deployment',GETDATE()),
				(12,'RouteHeader','RouteDate','deployment',GETDATE(),'deployment',GETDATE())
		)
AS Source ([Id],[ObjectType],[AttributeName],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[ObjectType],[AttributeName],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Id],[ObjectType],[AttributeName],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]);

SET IDENTITY_INSERT [RouteAttributeExceptions] OFF
