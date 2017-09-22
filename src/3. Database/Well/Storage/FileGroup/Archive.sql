/*
Do not change the database path or name variables.
Any sqlcmd variables will be properly substituted during 
build and deployment.
*/
ALTER DATABASE [$(DatabaseName)]
	ADD FILEGROUP [Archive]
GO
ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Archive], FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_Archive.mdf') TO FILEGROUP [Archive];
