CREATE TABLE [dbo].[DeliveryAction]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Description] VARCHAR(50) NULL,
	CONSTRAINT [PK_DeliveryAction] PRIMARY KEY CLUSTERED ([Id] ASC)
)
