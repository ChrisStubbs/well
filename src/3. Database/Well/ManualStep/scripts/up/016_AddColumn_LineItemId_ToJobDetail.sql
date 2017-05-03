ALTER TABLE dbo.JobDetail
ADD LineItemId INT NULL CONSTRAINT [FK_JobDetail_LineItem] FOREIGN KEY ([LineItemId]) REFERENCES [dbo].[LineItem] ([Id]) 

