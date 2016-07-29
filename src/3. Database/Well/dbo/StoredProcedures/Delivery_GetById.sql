CREATE PROCEDURE [dbo].[Delivery_GetById]
	@Id INT
AS
SELECT j.[Id] AS [Id]
	,j.JobRef1 AS AccountCode
	,a.Name AS AccountName
	,a.Address1 + ',' + a.Address2  + ',' + a.PostCode AS AccountAddress
	,j.JobRef3 AS InvoiceNumber
	,a.ContactName
	,a.ContactNumber AS PhoneNumber
	,a.ContactNumber2 AS MobileNumber
	,ps.[Description] AS DeliveryType
FROM [dbo].Job j
JOIN [dbo].PerformanceStatus ps on ps.Id = j.PerformanceStatusId
JOIN [dbo].[Account] a on a.StopId = j.StopId
 
WHERE j.Id = @Id 
