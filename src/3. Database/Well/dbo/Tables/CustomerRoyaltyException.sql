CREATE TABLE [dbo].[CustomerRoyaltyException]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[RoyaltyCode] INT NOT NULL,
	[Customer] VARCHAR(100) NOT NULL,
	[ExceptionDays] TINYINT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_CustomerRoyaltyException] PRIMARY KEY CLUSTERED ([Id] ASC)
)
