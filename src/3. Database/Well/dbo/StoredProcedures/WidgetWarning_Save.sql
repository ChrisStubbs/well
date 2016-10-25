CREATE PROCEDURE [dbo].[WidgetWarning_Save]
	@WidgetName VARCHAR(50),
	@WarningLevel INT,
	@Type TINYINT,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].Widget(Description, WarningLevel, [Type], CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	VALUES (@WidgetName, @WarningLevel, @Type, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

	SELECT CAST(SCOPE_IDENTITY() as int);

END
