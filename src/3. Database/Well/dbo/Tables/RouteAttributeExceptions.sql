CREATE TABLE [dbo].[RouteAttributeExceptions]
(
	[Id] INT NOT NULL, 
    [EpodType] INT NOT NULL, 
    [ObjectType] VARCHAR(20) NOT NULL, 
    [AttributeName] VARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_RouteAttributeExceptions] PRIMARY KEY CLUSTERED ([Id] ASC),
)
