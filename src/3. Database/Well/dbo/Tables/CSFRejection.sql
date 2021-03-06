﻿CREATE TABLE [dbo].[CSFRejection]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[RejectionReason] VARCHAR(30),
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_CSFRejection] PRIMARY KEY CLUSTERED ([Id] ASC),
)
