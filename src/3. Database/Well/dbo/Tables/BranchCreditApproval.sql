CREATE TABLE [dbo].[BranchCreditApproval]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[BranchCode] VARCHAR(20) NOT NULL,
	[MinimumCreditValue] DECIMAL(5,2) NOT NULL,
	[MaximumCreditValue] DECIMAL(5,2) NULL,
	[MinimumApprovalValue] DECIMAL(5,2) NOT NULL,
	[MaximumApprovalValue] DECIMAL(5,2) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_BranchCreditApproval] PRIMARY KEY CLUSTERED ([Id] ASC),
)
