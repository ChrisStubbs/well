CREATE PROCEDURE [dbo].[Account_Update]
	@Id AS int
	,@Code as varchar(20)
	,@AccountTypeCode as varchar(20)
	,@DepotId as int
	,@Name as varchar(50)
	,@Address1 as varchar(50)
	,@Address2 as varchar(50)
	,@PostCode as varchar(10)
	,@ContactName as varchar(50)
	,@ContactNumber as varchar(15)
	,@ContactNumber2 as varchar(15)
	,@ContactEmailAddress as varchar(50)
	,@StopId as int
	,@UpdatedBy as varchar(50)
	,@DateUpdated as datetime
	,@DateDeleted as datetime = null
	,@LocationId AS int
AS

	UPDATE [dbo].[Account]
	   SET [Code] = @Code
		  ,[AccountTypeCode] = @AccountTypeCode
		  ,[DepotId] = @DepotId
		  ,[Name] = @Name
		  ,[Address1] = @Address1
		  ,[Address2] = @Address2
		  ,[PostCode] = @PostCode
		  ,[ContactName] = @ContactName
		  ,[ContactNumber] = @ContactNumber
		  ,[ContactNumber2] = @ContactNumber2
		  ,[ContactEmailAddress] = @ContactEmailAddress
		  ,[DateDeleted] = @DateDeleted
		  ,[StopId] = @StopId
		  ,[UpdatedBy] = @UpdatedBy
		  ,[DateUpdated] = @DateUpdated
		  ,[LocationId] = @LocationId
	WHERE
		Id = @Id
