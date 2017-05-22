CREATE PROCEDURE [dbo].[Account_GetByAccountCode]
	@Code NVARCHAR(20),
	@StopId INT
AS
BEGIN

	SET NOCOUNT ON;

SELECT [Id]
      ,[Code]
      ,[AccountTypeCode]
      ,[DepotId]
      ,[Name]
      ,[Address1]
      ,[Address2]
      ,[PostCode]
      ,[ContactName]
      ,[ContactNumber]
      ,[ContactNumber2]
      ,[ContactEmailAddress]
      ,[StopId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
 FROM [dbo].[Account]
 WHERE [Code] = @Code
 AND [StopId] = @StopId


END
