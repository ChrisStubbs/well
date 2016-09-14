CREATE PROCEDURE [dbo].[Audit_Get]

AS
	SELECT [Id]
      ,[Entry]
      ,[Type]
      ,[InvoiceNumber]
      ,[AccountCode]
      ,[AccountName]
      ,[DeliveryDate]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[Audit]
RETURN 0
