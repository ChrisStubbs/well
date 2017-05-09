﻿CREATE TABLE [dbo].[Activity]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[DocumentNumber] VARCHAR(40) NOT NULL,
	[ActivityTypeId] TINYINT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Activity_ActivityType] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
