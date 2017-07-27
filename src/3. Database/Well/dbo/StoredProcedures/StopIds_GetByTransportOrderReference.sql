CREATE PROCEDURE [dbo].[StopIds_GetByTransportOrderReference]
	@transportOrderReference	dbo.StringTableType	READONLY
AS
BEGIN

  SELECT 
	  [Id]
  FROM [dbo].[Stop] s 
	INNER JOIN @transportOrderReference tor ON tor.Value = s.TransportOrderReference
  WHERE
	 s.DeliveryDate IS NULL

END