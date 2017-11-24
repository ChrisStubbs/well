CREATE TABLE [dbo].[CreditThreshold]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Level] INT NOT NULL,
	[Threshold] DECIMAL(10,2) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL, 
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_CreditThreshold] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT UC_CreditThreshold UNIQUE ([Level])
)
