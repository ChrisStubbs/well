CREATE TABLE [dbo].[Routes]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[FileName] VARCHAR(255),
	[DateDeleted] DATETIME NULL, 
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED ([Id] ASC),
)


