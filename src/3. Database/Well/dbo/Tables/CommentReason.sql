CREATE TABLE [dbo].[CommentReason]
(
		[Id] INT IDENTITY(1,1) NOT NULL,
		[Description] VARCHAR(100) NOT NULL,
		[CreatedBy] VARCHAR(50) NOT NULL,
		[DateCreated] DATETIME NOT NULL,
		[UpdatedBy] VARCHAR(50) NOT NULL,
		[DateUpdated] DATETIME NOT NULL,
		[Version] [TIMESTAMP] NOT NULL,
		CONSTRAINT [PK_CommentReason] PRIMARY KEY CLUSTERED ([Id] ASC),
)
