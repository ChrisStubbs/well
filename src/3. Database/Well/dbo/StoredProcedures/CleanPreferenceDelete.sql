CREATE PROCEDURE [dbo].[CleanPreferenceDelete]
	@Id INT
AS
BEGIN

DELETE FROM
	 [dbo].[CleanPreferenceToBranch]
WHERE
	CleanPreferenceId = @Id

DELETE FROM
	 [dbo].[CleanPreference]
WHERE
	Id = @Id
		   		   
END