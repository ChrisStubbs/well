CREATE TYPE EventTableType AS TABLE
(
    [Event]             VARCHAR(MAX) NOT NULL,
	ExceptionActionId   INT NOT NULL,
	DateCanBeProcessed  DATETIME NOT NULL,
	CreatedBy           VARCHAR(50) NOT NULL,
	DateCreated         DATETIME NOT NULL,
	UpdatedBy           VARCHAR(50) NOT NULL,
	DateUpdated         DATETIME NOT NULL
)