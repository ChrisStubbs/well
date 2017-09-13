CREATE PROCEDURE [dbo].[Routes_CheckDuplicate]
	@FileName VARCHAR(50)

AS
BEGIN
	SELECT [FileName] FROM [Routes] WHERE [FileName] = @FileName
END
