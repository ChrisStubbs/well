CREATE TABLE [dbo].[WidgetToBranch]
(
	[Widget_Id] INT NOT NULL,
    [Branch_Id] INT NOT NULL,
	CONSTRAINT [FK_WidgetToBranch_Branch_Id] FOREIGN KEY ([Branch_Id]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Widget_Id] FOREIGN KEY ([Widget_Id]) REFERENCES [dbo].[Widget] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
