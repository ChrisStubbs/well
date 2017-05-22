CREATE TABLE [dbo].[DamageSource]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
	[Description] VARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_DamageSource] PRIMARY KEY CLUSTERED ([Id] ASC),
)
