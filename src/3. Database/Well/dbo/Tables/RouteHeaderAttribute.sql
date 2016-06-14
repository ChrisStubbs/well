CREATE TABLE [dbo].[RouteHeaderAttribute]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] NVARCHAR(10) NOT NULL,
	[Value] NVARCHAR(100) NOT NULL,
	[RouteHeaderId] INT NOT NULL,
	CONSTRAINT [PK_RouteHeaderAttribute] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RouteHeaderAttribute_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteImport] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
