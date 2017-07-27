IF OBJECT_ID('StopsGetById', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE StopsGetById
END

GO

IF OBJECT_ID('Stop_GetByOrderUpdateDetails', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE Stop_GetByOrderUpdateDetails
END
