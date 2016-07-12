﻿CREATE TABLE [dbo].[JobDetailAttribute]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Code] NVARCHAR(10) NOT NULL,
	[Value] NVARCHAR(100) NOT NULL,
	[JobDetailId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_JobDetailAttribute] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailAttribute_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
