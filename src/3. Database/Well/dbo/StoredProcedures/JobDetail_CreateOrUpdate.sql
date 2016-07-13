CREATE PROCEDURE [dbo].[JobDetail_CreateOrUpdate]
	@Id						INT = 0,
	@LineNumber				INT,
	@Username				VARCHAR(50),
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
	@JobId					INT

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [JobDetail] AS Target
	USING (VALUES
		(@Id, @LineNumber, @Barcode, @OriginalDespatchQty, @ProdDesc, @OrderedQty,@ShortQty, @SkuWeight, @SkuCube, @UnitMeasure, @TextField1, @TextField2,
		 @TextField3, @TextField4, @TextField5,@SkuGoodsValue, @JobId, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[LineNumber],[Barcode],[OriginalDespatchQty], [ProdDesc], [OrderedQty],[ShortQty], [SkuWeight], [SkuCube], [UnitMeasure],
				[TextField1], [TextField2],[TextField3],[TextField4],[TextField5], [SkuGoodsValue], [JobId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[LineNumber] = Source.[LineNumber],
		[Barcode] = Source.[Barcode],
		[OriginalDespatchQty] = Source.[OriginalDespatchQty],
		[ProdDesc] = Source.[ProdDesc],
		[OrderedQty] = Source.[OrderedQty],
		[ShortQty] = Source.[ShortQty],
		[SkuWeight] = Source.[SkuWeight],
		[SkuCube] = Source.[SkuCube],
		[UnitMeasure] = Source.[UnitMeasure],
		[TextField1] = Source.[TextField1],
		[TextField2] = Source.[TextField2],
		[TextField3] = Source.[TextField3],
		[TextField4] = Source.[TextField4],
		[TextField5] = Source.[TextField5],
		[SkuGoodsValue] = Source.[SkuGoodsValue],
		[JobId] = Source.[JobId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([LineNumber],[Barcode],[OriginalDespatchQty], [ProdDesc], [OrderedQty],[ShortQty], [SkuWeight], [SkuCube], [UnitMeasure],
				[TextField1], [TextField2],[TextField3],[TextField4],[TextField5], [SkuGoodsValue], [JobId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([LineNumber],[Barcode],[OriginalDespatchQty], [ProdDesc], [OrderedQty],[ShortQty], [SkuWeight], [SkuCube], [UnitMeasure],
				[TextField1], [TextField2], [TextField3],[TextField4],[TextField5],[SkuGoodsValue], [JobId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END
