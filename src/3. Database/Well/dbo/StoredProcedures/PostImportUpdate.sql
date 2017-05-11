CREATE PROCEDURE [dbo].[PostImportUpdate]
AS
BEGIN
	
	EXECUTE Location_Insert
	EXECUTE Activity_InsertUpdate
	EXECUTE LineItem_InsertUpdate
	EXECUTE Bag_InsertUpdate

END