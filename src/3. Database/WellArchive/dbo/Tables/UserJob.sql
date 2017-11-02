CREATE TABLE [dbo].[UserJob]
(
	[Id]			INT NOT NULL, 
	[UserId]		INT NOT NULL,
	[JobId]			INT NOT NULL,
	[CreatedBy]		VARCHAR(50) NOT NULL,
	[DateCreated]	DATETIME NOT NULL,
	[UpdatedBy]		VARCHAR(50) NOT NULL,
	[DateUpdated]	DATETIME NOT NULL,
	[ArchiveDate]	SMALLDATETIME NOT NULL
)
