CREATE TABLE [dbo].[Account]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
	[AccountTypeCode] VARCHAR(20) NOT NULL,
	[DepotId] INT  NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[PostCode]	VARCHAR(10) NOT NULL,
	[ContactName] VARCHAR(50) NOT NULL,
	[ContactNumber] VARCHAR(15) NOT NULL,
	[ContactNumber2] VARCHAR(15) NULL,
	[ContactEmailAddress] VARCHAR(50) NULL,
	[DateDeleted] DATETIME NULL, 
	[StopId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[LocationId] INT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Account_Stop] FOREIGN KEY ([StopId]) REFERENCES [dbo].[Stop] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Account_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
-- used for search
CREATE NONCLUSTERED INDEX [IDX_Account_StopId] ON [dbo].[Account] ([StopId])INCLUDE ([Code],[Name])
GO
CREATE NONCLUSTERED INDEX [Idx_Account_LocationId] ON [dbo].[Account] ([LocationId])
GO
