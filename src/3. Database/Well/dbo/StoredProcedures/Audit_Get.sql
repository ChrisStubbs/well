CREATE PROCEDURE [dbo].[Audit_Get]

AS
	SELECT a.[Id]
      ,a.[Entry]
      ,a.[Type]
      ,a.[InvoiceNumber]
      ,a.[AccountCode] 
	  ,[Account].Name as AccountName    
      ,a.[DeliveryDate]
      ,a.[CreatedBy]
      ,a.[DateCreated]
      ,a.[UpdatedBy]
      ,a.[DateUpdated]
      ,a.[Version]
  FROM [dbo].[Audit] a
  left Join [dbo].[Account] on [Account].Code = a.[AccountCode]
RETURN 0
