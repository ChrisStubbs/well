CREATE PROCEDURE [dbo].[JobDetail_Insert]
	@LineNumber				INT,
	@Barcode				VARCHAR(60),
	@OriginalDespatchQty	DECIMAL(7,3),
	@ProdDesc				NVARCHAR(100),
	@OrderedQty				INT=NULL,
	@ShortQty				INT=NULL,
	@SkuWeight				DECIMAL(7,3),
	@SkuCube				DECIMAL(7,3),
	@UnitMeasure			NVARCHAR(20),
	@TextField1				NVARCHAR(50),
	@TextField2				NVARCHAR(50),
	@TextField3				NVARCHAR(50)=NULL,
	@TextField4				NVARCHAR(50)=NULL,
	@TextField5				NVARCHAR(50)=NULL,
	@SkuGoodsValue			FLOAT,
	@JobId					INT,
	@JobDetailStatusId		INT,
	@CreatedBy				VARCHAR(50),
	@DateCreated			Datetime,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO [dbo].[JobDetail]
           ([LineNumber]
           ,[Barcode]
           ,[OriginalDespatchQty]
           ,[ProdDesc]
           ,[OrderedQty]
           ,[ShortQty]
           ,[SkuWeight]
           ,[SkuCube]
           ,[UnitMeasure]
           ,[TextField1]
           ,[TextField2]
           ,[TextField3]
           ,[TextField4]
           ,[TextField5]
           ,[SkuGoodsValue]
           ,[JobId]
           ,[JobDetailStatusId]
           ,[IsDeleted]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
     VALUES
           (@LineNumber
           ,@Barcode
           ,@OriginalDespatchQty
           ,@ProdDesc
           ,@OrderedQty
           ,@ShortQty
           ,@SkuWeight
           ,@SkuCube
           ,@UnitMeasure
           ,@TextField1
           ,@TextField2
           ,@TextField3
           ,@TextField4
           ,@TextField5
           ,@SkuGoodsValue
           ,@JobId
           ,@JobDetailStatusId
           ,0
           ,@CreatedBy
           ,@DateCreated
           ,@UpdatedBy
           ,@DateUpdated)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
