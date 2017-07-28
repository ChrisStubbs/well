CREATE PROCEDURE CleanJobsSetResolutionStatusClosed
    @JobIds IntTableType READONLY,
    @DateDeleted DateTime,
	@UpdatedBy VARCHAR(50)
AS
   

    /************************************************/
    /*** Set resolution status close to open jobs ***/
    /************************************************/
    UPDATE JOB
    SET ResolutionStatusId = ISNULL(NULLIF(ResolutionStatusId, 1), 2) | 256 --if job is status = imported set it to driver completed 
    WHERE 
        Job.Id IN (SELECT value FROM @JobIds)
        AND job.ResolutionStatusId IN (1, 2, 512)

  