CREATE TYPE IdentifyJobTableType AS TABLE
(
	[PHAccount] [varchar](40) NOT NULL,
	[PickListRef] [varchar](40) NULL,
	[JobTypeCode] [varchar](10) NOT NULL
)