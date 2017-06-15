CREATE TABLE [dbo].[Widget]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Type] TINYINT NOT NULL,
    [Description] VARCHAR(50) NOT NULL,
	[WarningLevel] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL, 
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Widget] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Widget_Type] FOREIGN KEY ([Type]) REFERENCES [dbo].[WidgetType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,

)
