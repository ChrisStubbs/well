USE [master]
GO
ALTER DATABASE [Well] MODIFY FILE ( NAME = N'Archive', FILEGROWTH = 10%)
GO
ALTER DATABASE [Well] MODIFY FILE ( NAME = N'Well', FILEGROWTH = 10%)
GO

-- Takes around 6 Minutes
USE Well
GO

Declare @loopNo INTEGER = 0
WHILE ( (Select Count(1) From Job Where DateDeleted Is Not Null) > 0 )
BEGIN
	SET @loopNo = @loopNo +1;
	Print 'Loop' +  CAST(@loopNo as varchar(max));
	DECLARE @jobIds as IntTableType,
			@archiveDate as datetime = getdate()
	insert into @jobIds (Value) select Top 20000 id from Job Where DateDeleted Is Not Null
	
	Begin tran
		Print 'CleanJobsSetResolutionStatusClosed';
		exec CleanJobsSetResolutionStatusClosed @jobIds, @archiveDate, 'Data Archive'
		Print 'Archive_Jobs';
		exec Archive_Jobs @jobIds, @archiveDate;
		Print 'Archive_Stops';
		exec Archive_Stops @archiveDate
		Print 'Archive_Routes';
		exec Archive_Routes @archiveDate
		Print 'Archive_Activity';
		exec Archive_Activity @archiveDate
	Commit Tran
END;

exec sp_updatestats