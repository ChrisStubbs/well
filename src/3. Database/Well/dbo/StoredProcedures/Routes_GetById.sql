﻿CREATE PROCEDURE [dbo].[Routes_GetById]
	@Id INT
AS
BEGIN
	SELECT 
	   [Id],
       [FileName],
       [CreatedBy],
       [DateCreated],
       [UpdatedBy],
       [DateUpdated],
       [Version]
  FROM [dbo].[Routes]
  WHERE [Id] = @Id
END
