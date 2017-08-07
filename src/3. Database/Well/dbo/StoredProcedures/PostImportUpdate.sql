CREATE PROCEDURE [dbo].[PostImportUpdate]
	@JobIds dbo.IntTableType READONLY
AS
BEGIN
	
	EXECUTE Location_Insert @JobIds
	EXECUTE Activity_InsertUpdate @JobIds
	EXECUTE LineItem_InsertUpdate @JobIds
	EXECUTE Bag_InsertUpdate @JobIds

END