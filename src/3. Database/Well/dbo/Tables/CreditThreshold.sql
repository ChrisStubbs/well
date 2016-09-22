CREATE TABLE [dbo].[CreditThreshold]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UserRoleId] INT NOT NULL,
	[Threshold] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_CreditThreshold] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_CreditThreshold_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
