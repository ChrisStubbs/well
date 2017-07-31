CREATE TYPE IdentifyJobTableType AS TABLE
(
	[JobTypeCode] [varchar](10) NOT NULL,
	[PHAccount] [varchar](40) NOT NULL,
	[PickListRef] [varchar](40) NULL
)