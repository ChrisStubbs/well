﻿CREATE TABLE [dbo].[JobType]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
	[Description] VARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[ActivityTypeId] TINYINT NULL,
    [Abbreviation] CHAR(3) NULL, 
    CONSTRAINT [PK_JobType] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobType_ActivityType] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
