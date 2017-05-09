ALTER TABLE dbo.JobType
ADD ActivityTypeId TINYINT NULL CONSTRAINT [FK_JobType_ActivityType] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([Id]) 

