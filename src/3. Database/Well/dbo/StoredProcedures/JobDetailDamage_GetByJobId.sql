CREATE PROCEDURE [dbo].[JobDetailDamage_GetByJobDetailId]
	@JobDetailId int
AS
SELECT [Id]
      ,[JobDetailId]
      ,[Qty]
      ,[IsDeleted]
      ,[DamageReasonsId]
      ,[DamageSourceId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
  FROM [Well].[dbo].[JobDetailDamage]
  WHERE [JobDetailId] = @JobDetailId
RETURN 0


