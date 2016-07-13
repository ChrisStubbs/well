CREATE TABLE [dbo].[RouteHeaderAttribute]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] NVARCHAR(10) NOT NULL,
	[Value] NVARCHAR(100) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[RouteHeaderId] INT NOT NULL,
	CONSTRAINT [PK_RouteHeaderAttribute] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RouteHeaderAttribute_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteHeader] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
