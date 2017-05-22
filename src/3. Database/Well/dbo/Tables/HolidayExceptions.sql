CREATE TABLE [dbo].[HolidayExceptions]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
    [ExceptionDate] DATETIME NOT NULL, 
    [Exception] NVARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_HolidayExceptions] PRIMARY KEY CLUSTERED ([Id] ASC),
)
