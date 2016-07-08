﻿CREATE TABLE [dbo].[Job]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Sequence] INT NOT NULL,
	[JobTypeCode] NVARCHAR(10) NOT NULL,
	[JobRef1] NVARCHAR(40) NOT NULL,
	[JobRef2] NVARCHAR(40) NULL,
	[JobRef3] NVARCHAR(40) NOT NULL,
	[JobRef4] NVARCHAR(40) NULL,
	[OrderDate] DATETIME NOT NULL,
	[Originator] NVARCHAR(10) NULL,
	[TextField1] NVARCHAR(50) NULL,
	[TextField2] NVARCHAR(50) NULL,
	[PerformanceStatusCode] NVARCHAR(5) NULL,
	[StopId] INT NOT NULL,
	CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Job_Stop] FOREIGN KEY ([StopId]) REFERENCES [dbo].[Stop] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
