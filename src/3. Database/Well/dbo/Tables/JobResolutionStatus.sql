CREATE TABLE JobResolutionStatus
(
	Id					Int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_JobResolutionStatus PRIMARY KEY CLUSTERED,
	[Status]			VarChar(30) NOT NULL,
	Job					Int NOT NULL CONSTRAINT FK_Job_JobResolutionStatus FOREIGN KEY REFERENCES Job(Id),
	[By]				VARCHAR(50) NOT NULL,
	[On]				SmallDateTime NOT NULL
)
GO
CREATE NONCLUSTERED INDEX [Idx_JobResolutionStatus_Job] ON [dbo].[JobResolutionStatus] ([Job]) INCLUDE ([Id],[Status],[By],[On])
