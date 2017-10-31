CREATE PROCEDURE [dbo].[Routes_FileNameCount]
	@FileName varchar(255)
AS
	SELECT Count(1) As FileNameCount From dbo.Routes Where FileName = @FileName

