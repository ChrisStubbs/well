CREATE TABLE [dbo].[CustomerRoyaltyException]
(
	[RoyaltyId] INT IDENTITY(1,1) NOT NULL,
	[Customer] VARCHAR(100) NOT NULL,
	[ExceptionDays] INT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_CustomerRoyaltyException] PRIMARY KEY CLUSTERED ([RoyaltyId] ASC)
)
