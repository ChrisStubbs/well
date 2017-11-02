CREATE TABLE [dbo].[ExceptionEvent]
(
	[Id] INT NOT NULL,
	[Event] [varchar](max) NOT NULL,
	[ExceptionActionId] [int] NOT NULL,
	[Processed] [bit] NOT NULL,
	[DateCanBeProcessed] [datetime] NOT NULL,
	[SourceId] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[UpdatedBy] [varchar](50) NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Version] [timestamp] NOT NULL,
	[ArchiveDate] SMALLDATETIME NOT NULL
)
