CREATE PROCEDURE [dbo].[StopIds_GetByTransportOrderReference]
	@transportOrderReferences	dbo.StringTableType	READONLY
AS
BEGIN

  SELECT 
	  [Id]
  FROM [dbo].[Stop] s 
	INNER JOIN @transportOrderReferences tor ON tor.Value = s.TransportOrderReference

END