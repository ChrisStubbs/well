CREATE TABLE [dbo].[BranchApprovalTime]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[BranchCode] VARCHAR(20) NOT NULL,
	[MaximumApprovalTime] TINYINT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_BranchApprovalTime] PRIMARY KEY CLUSTERED ([Id] ASC),
)
