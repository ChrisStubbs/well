-- This script is targeting 2 exception events in production that will never be processed 
Delete 
  FROM [Well].[dbo].[ExceptionEvent]
  WHERE Id IN(13112,17088)