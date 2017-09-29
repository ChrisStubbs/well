BEGIN

	DELETE lia 
	FROM LineItemAction  lia
	INNER JOIN LineItem li on lia.LineItemId = li.Id
	INNER JOIN Job j on li.JobId = j.Id
	WHERE j.jobTypeCode = 'UPL-GLO'

	UPDATE Job
	SET ResolutionStatusId = 2 --  Driver Completed
	WHERE ResolutionStatusId = 4 -- Action Required
	AND jobTypeCode = 'UPL-GLO'

END
GO