CREATE TABLE [dbo].JobResolutionStatus
(
	Id				INT NOT NULL,
	DataSource		VarChar(255) NULL,
	[Status]		VARCHAR(30) NOT NULL,
	Job				INT NOT NULL,
	[By]			VARCHAR(50) NOT NULL,
	[On]			SMALLDATETIME NOT NULL,
	[ArchiveDate]	SMALLDATETIME NOT NULL
)