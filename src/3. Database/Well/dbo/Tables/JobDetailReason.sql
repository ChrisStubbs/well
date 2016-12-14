CREATE TABLE [dbo].[JobDetailReason]
(
	Id TINYINT IDENTITY(1, 1) NOT NULL,
	[Description] VarChar(50) NOT NULL, 
	CONSTRAINT [PK_JobDetailReason] PRIMARY KEY CLUSTERED ([Id] ASC)
)