CREATE PROCEDURE [dbo].[JobDetail_Update]
	@Id						Int,
	@DeliveredQty			DECIMAL(8,3) ,
	@ShortQty				INT,
	@JobDetailStatusId		INT,
	@LineDeliveryStatus		INT,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			DATETIME

AS
BEGIN
	SET NOCOUNT ON;

UPDATE 
		[dbo].[JobDetail]
   SET 
	   [DeliveredQty] = @DeliveredQty
      ,[ShortQty] = @ShortQty    
      ,[JobDetailStatusId] = @JobDetailStatusId
	  ,[LineDeliveryStatus] = @LineDeliveryStatus
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
 WHERE 
	[Id] = @Id
END
