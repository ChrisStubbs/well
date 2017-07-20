IF OBJECT_ID('dbo.RoyaltyExceptions', 'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.RoyaltyExceptions; 
END

IF OBJECT_ID('dbo.CustomerRoyaltyException', 'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.CustomerRoyaltyException; 
END
  