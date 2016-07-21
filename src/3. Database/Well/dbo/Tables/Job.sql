﻿CREATE TABLE [dbo].[Job]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Sequence] INT NOT NULL,
	[JobTypeCode] VARCHAR(10) NOT NULL,
	[JobRef1] VARCHAR(40) NOT NULL,
	[JobRef2] VARCHAR(40) NULL,
	[JobRef3] VARCHAR(40) NOT NULL,
	[JobRef4] VARCHAR(40) NULL,
	[OrderDate] DATETIME NOT NULL,
	[Originator] VARCHAR(10) NULL,
	[TextField1] VARCHAR(50) NULL,
	[TextField2] VARCHAR(50) NULL,
	[PerformanceStatusId] TINYINT NULL,
	[ByPassReasonId] TINYINT NULL,
	[StopId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Job_Stop] FOREIGN KEY ([StopId]) REFERENCES [dbo].[Stop] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
