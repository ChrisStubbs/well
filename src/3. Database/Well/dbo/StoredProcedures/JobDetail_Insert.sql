CREATE PROCEDURE [dbo].[JobDetail_Insert]
	@LineNumber				INT,
	@OriginalDespatchQty	INT,
	@ProdDesc				NVARCHAR(100),
	@OrderedQty				INT,
	@DeliveredQty				DECIMAL(8,3) ,
	@ShortQty				INT,
	@UnitMeasure			NVARCHAR(20),
	@PHProductCode			NVARCHAR(50),
	@PHProductType			VARCHAR(50) NULL,
	@PackSize				NVARCHAR(50),
	@SingleOrOuter			NVARCHAR(50)=NULL,
	@SSCCBarcode			NVARCHAR(50)=NULL,
	@SubOuterDamageTotal	INT=NULL,
	@SkuGoodsValue			FLOAT,
	@NetPrice			FLOAT,
	@JobId					INT,
	@JobDetailStatusId		INT,
	@CreatedBy				VARCHAR(50),
	@DateCreated			Datetime,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime,
	@IsDeleted				BIT = 0

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO [dbo].[JobDetail]
           ([LineNumber]
		   ,[OriginalDespatchQty]
		   ,[ProdDesc]
		   ,[OrderedQty]
		   ,[DeliveredQty]
           ,[ShortQty]
           ,[UnitMeasure]
           ,[PHProductCode]
           ,[PHProductType]
           ,[PackSize]
           ,[SingleOrOuter]
           ,[SSCCBarcode]
		   ,[SubOuterDamageTotal]
           ,[SkuGoodsValue]
		   ,[NetPrice]
           ,[JobId]
           ,[JobDetailStatusId]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated]
		   ,[IsDeleted])
     VALUES
           (@LineNumber
		   ,@OriginalDespatchQty
		   ,@ProdDesc
           ,@OrderedQty
		   ,@DeliveredQty
           ,@ShortQty
           ,@UnitMeasure
           ,@PHProductCode     
		   ,@PHProductType
		   ,@PackSize			
		   ,@SingleOrOuter			
		   ,@SSCCBarcode			
		   ,@SubOuterDamageTotal	
           ,@SkuGoodsValue
		   ,@NetPrice
           ,@JobId
           ,@JobDetailStatusId
           ,@CreatedBy
           ,@DateCreated
           ,@UpdatedBy
           ,@DateUpdated
		   ,@IsDeleted)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
